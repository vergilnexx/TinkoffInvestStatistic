using Contracts;
using Contracts.Enums;
using Infrastructure.Clients;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class AccountService : IAccountService
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Account>> GetAccountsAsync()
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var externalAccounts = await bankBrokerClient.GetAccountsAsync();

            var positionService = DependencyService.Resolve<IPositionService>();
            foreach (var account in externalAccounts)
            {
                account.Sum = await positionService.GetPositionsSumAsync(account.ID);
            }

            var accounts = await DataStorageService.Instance.MergeAccountData(externalAccounts);

            return accounts.ToArray();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<AccountCurrencyData>> GetAccountDataByCurrenciesTypes(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            IEnumerable<Position> positions = await bankBrokerClient.GetPositionsAsync(accountId);
            var currencies = await bankBrokerClient.GetCurrenciesAsync();

            // Данные в позициях о валютах не подходят, так как они представляют курс, а не размер кэша, поэтому кэш добавляем отдельно.
            positions = positions.Where(p => p.Type != PositionType.Currency).ToArray();
            foreach (var position in positions)
            {
                var expectedYield = position.ExpectedYield?.Sum ?? 0;
                position.SumInCurrency = position.PositionCount * (position.AveragePositionPrice?.Sum ?? 0) + expectedYield;

                // Если цена не в рублях рассчитываем по текущему курсу.
                if (position.AveragePositionPrice?.Currency != Currency.Rub)
                {
                    var currency = currencies.FirstOrDefault(c => position.AveragePositionPrice?.Currency == c.Currency);
                    if (currency == null)
                    {
                        throw new ApplicationException("Не найдена валюта типа: " + position.AveragePositionPrice?.Currency);
                    }

                    position.Sum = position.SumInCurrency * currency.Sum;
                    position.DifferenceSum = expectedYield * currency.Sum;
                }
                else
                {
                    position.Sum = position.SumInCurrency;
                    position.DifferenceSum = expectedYield;
                }
            }

            var sum = positions.Sum(p => p.Sum);
            var fiatPositions = await bankBrokerClient.GetFiatPositionsAsync(accountId);
            foreach (var currencyPosition in fiatPositions)
            {
                if (currencyPosition.Currency != Currency.Rub)
                {
                    var currency = currencies.FirstOrDefault(c => currencyPosition.Currency == c.Currency);
                    if (currency == null)
                    {
                        throw new ApplicationException("Не найдена валюта типа: " + currency);
                    }

                    currencyPosition.Sum *= currency.Sum;
                }
            }
            var currenciesPositions = fiatPositions
                .Select(fp => new Position(string.Empty, PositionType.Currency, fp.Currency.GetDescription(), fp.Sum) { AveragePositionPrice = new CurrencyMoney(fp.Currency, 0) });

            var sum2 = currenciesPositions.Sum(p => p.Sum);
            positions = positions.Union(currenciesPositions).ToArray();
            var result = positions
                            .GroupBy(p => p.AveragePositionPrice.Currency)
                            .Select(currencyGroup => new AccountCurrencyData(currencyGroup.Key, 0, currencyGroup.Sum(cg => cg.Sum)))
                            .ToArray();
            return result;
        }
    }
}

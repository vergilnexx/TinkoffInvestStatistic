using Contracts;
using Contracts.Enums;
using Infrastructure.Clients;
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

            var fiatPositions = await bankBrokerClient.GetFiatPositionsAsync(accountId);
            var rubles = fiatPositions
                .Where(fp => fp.Currency == Currency.Rub)
                .Select(fp => new Position(string.Empty, PositionType.Currency, "Рубль", fp.Value) { AveragePositionPrice = new CurrencyMoney(Currency.Rub, 0) });

            // Добавляем данные про кэш в рублях.
            positions = positions.Union(rubles).ToArray();
            foreach (var position in positions)
            {
                position.SumInCurrency = position.PositionCount * (position.AveragePositionPrice?.Value ?? 0) + (position.ExpectedYield?.Value ?? 0);

                // Если цена не в рублях рассчитываем по текущему курсу.
                if (position.AveragePositionPrice?.Currency != Currency.Rub)
                {
                    var currency = currencies.FirstOrDefault(c => position.AveragePositionPrice?.Currency == c.Currency);
                    if (currency == null)
                    {
                        throw new ApplicationException("Не найдена валюта типа: " + position.AveragePositionPrice?.Currency);
                    }

                    position.Sum = position.SumInCurrency * currency.Value;
                    position.DifferenceSum = position.ExpectedYield.Value * currency.Value;
                }
                else
                {
                    position.Sum = position.SumInCurrency;
                    position.DifferenceSum = position.ExpectedYield.Value;
                }
            }
            
            var result = positions
                            .GroupBy(p => p.AveragePositionPrice.Currency)
                            .Select(currencyGroup => new AccountCurrencyData(currencyGroup.Key, 0, currencyGroup.Sum(cg => cg.Sum)))
                            .ToArray();
            return result;
        }
    }
}

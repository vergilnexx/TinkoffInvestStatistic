using Infrastructure.Clients;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;
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
            var brokerAccounts = await bankBrokerClient.GetAccountsAsync();

            foreach (var account in brokerAccounts)
            {
                var data = await bankBrokerClient.GetAccountsFullDataAsync(account.ID);
                account.Sum = data.TotalAmount?.Sum ?? 0m;
            }

            var accounts = await DataStorageService.Instance.MergeAccountData(brokerAccounts);

            return accounts.ToArray();
        }
        /// <inheritdoc/>
        public async Task<Portfolio> GetPortfolioAsync(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();

            return await bankBrokerClient.GetAccountsFullDataAsync(accountId);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<AccountCurrencyData>> GetAccountDataByCurrenciesTypes(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            Portfolio portfolio = await bankBrokerClient.GetAccountsFullDataAsync(accountId);

            // Группируем валюты и рассчитываем их стоимость.
            var cashInRoubles = portfolio.Positions
                                    .Where(p => p.Type == PositionType.Currency)
                                    .GroupBy(p => EnumHelper.GetCurrencyByFigi(p.Figi))
                                    .Where(p => p.Key.HasValue)
                                    .Select(g => KeyValuePair.Create(g.Key!.Value, g.Sum(i => i.Sum)))
                                    .ToArray();

            // Получаем курсы валют.
            var currencies = portfolio.Positions
                                .Where(p => p.Type == PositionType.Currency)
                                .Select(p => new { Currency = EnumHelper.GetCurrencyByFigi(p.Figi), Sum = p.CurrentPrice.Sum })
                                .Where(p => p.Currency != Currency.Rub)
                                .Select(p => KeyValuePair.Create(p.Currency!.Value, p.Sum))
                                .Union(new[] { KeyValuePair.Create(Currency.Rub, 1m) })
                                .ToList();

            // Группируем фин.инструменты по валюте и рассчитываем их стоимость в рублях.
            var instrumentsInRoubles = portfolio.Positions
                                        .Where(p => p.Type != PositionType.Currency)
                                        .GroupBy(p => p.Currency)
                                        .Select(g => KeyValuePair.Create(g.Key, g.Sum(i => i.SumInCurrency) * currencies.FirstOrDefault(c => c.Key == g.Key).Value))
                                        .ToArray();

            // Объединяем все рассчитанное.
            return cashInRoubles
                        .Union(instrumentsInRoubles)
                        .GroupBy(p => p.Key)
                        .ToDictionary(p => p.Key, p => p.Sum(s => s.Value))
                        .Select(currencyGroup => new AccountCurrencyData(currencyGroup.Key, currencyGroup.Value))
                        .ToArray();
        }

        //var currencies = await bankBrokerClient.GetCurrenciesAsync();

        //// Данные в позициях о валютах не подходят, так как они представляют курс, а не размер кэша, поэтому кэш добавляем отдельно.
        //portfolio = portfolio.Where(p => p.Type != PositionType.Currency).ToArray();
        //foreach (var position in portfolio)
        //{
        //    var expectedYield = position.ExpectedYield?.Sum ?? 0;
        //    position.SumInCurrency = position.PositionCount * (position.AveragePositionPrice?.Sum ?? 0) + expectedYield;

        //    // Если цена не в рублях рассчитываем по текущему курсу. 
        //    if (position.AveragePositionPrice?.Currency != Currency.Rub)
        //    {
        //        decimal? currencySum;
        //        var currency = currencies.FirstOrDefault(c => position.AveragePositionPrice?.Currency == c.Currency);
        //        if (currency == null)
        //        {
        //            currencySum = WorkaroundUtils.GetCurrencySumInRubbles(currencies, position.AveragePositionPrice?.Currency);
        //            if (currencySum == null)
        //            {
        //                throw new ApplicationException("Не найдена валюта типа: " + position.AveragePositionPrice?.Currency);
        //            }
        //        }
        //        else
        //        {
        //            currencySum = currency.Sum;
        //        }

        //        position.Sum = position.SumInCurrency * currencySum.Value;
        //        position.DifferenceSum = expectedYield * currencySum.Value;
        //    }
        //    else
        //    {
        //        position.Sum = position.SumInCurrency;
        //        position.DifferenceSum = expectedYield;
        //    }
        //}

        //var fiatPositions = await bankBrokerClient.GetFiatPositionsAsync(accountId);
        //foreach (var currencyPosition in fiatPositions)
        //{
        //    if (currencyPosition.Currency != Currency.Rub)
        //    {
        //        decimal? currencySum;
        //        var currency = currencies.FirstOrDefault(c => currencyPosition.Currency == c.Currency);
        //        if (currency == null)
        //        {
        //            currencySum = WorkaroundUtils.GetCurrencySumInRubbles(currencies, currencyPosition.Currency);
        //            if (currencySum == null)
        //            {
        //                throw new ApplicationException("Не найдена валюта типа: " + currencyPosition.Currency);
        //            }
        //        }
        //        else
        //        {
        //            currencySum = currency.Sum;
        //        }

        //        currencyPosition.Sum *= currencySum.Value;
        //    }
        //}
        //var currenciesPositions = fiatPositions
        //    .Select(fp => new Position(string.Empty, PositionType.Currency, fp.Currency.GetDescription(), fp.Sum) 
        //    { 
        //        AveragePositionPrice = new CurrencyMoney(fp.Currency, 0) 
        //    });

        //portfolio = portfolio.Union(currenciesPositions).ToArray();
        //var result = portfolio
        //                .GroupBy(p => p.AveragePositionPrice.Currency)
        //                .Select(currencyGroup => new AccountCurrencyData(currencyGroup.Key, 0, 
        //                                            currencyGroup.Sum(cg => cg.Sum)))
        //                .ToArray();
        //result = await DataStorageService.Instance.MergeCurrenciesData(accountId, result);
        //return result;
    }
}

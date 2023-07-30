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
            var dataByClient = cashInRoubles
                                .Union(instrumentsInRoubles)
                                .GroupBy(p => p.Key)
                                .ToDictionary(p => p.Key, p => p.Sum(s => s.Value))
                                .Select(currencyGroup => new AccountCurrencyData(currencyGroup.Key, currencyGroup.Value))
                                .ToArray();

            var mergedWithPlanned = await DataStorageService.Instance.MergeCurrenciesData(accountId, dataByClient);
            return mergedWithPlanned;
        }
    }
}

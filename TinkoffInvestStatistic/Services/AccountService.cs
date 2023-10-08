using Domain;
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

            var accounts = await MergeAccountDataAsync(brokerAccounts);

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
                                .Select(p => new { Currency = EnumHelper.GetCurrencyByFigi(p.Figi), Sum = p.CurrentPrice?.Sum ??0m })
                                .Where(p => p.Currency != Currency.Rub)
                                .Select(p => KeyValuePair.Create(p.Currency!.Value, p.Sum))
                                .Union(new[] { KeyValuePair.Create(Currency.Rub, 1m) })
                                .ToList();

            // Группируем фин.инструменты по валюте и рассчитываем их стоимость в рублях.
            var instrumentsInRoubles = portfolio.Positions
                                        .Where(p => p.Type != PositionType.Currency)
                                        .GroupBy(p => p.Currency)
                                        .Select(g => KeyValuePair.Create(g.Key, g.Sum(i => i.SumInCurrency) * currencies.Find(c => c.Key == g.Key).Value))
                                        .ToArray();

            // Объединяем все рассчитанное.
            var dataByClient = cashInRoubles
                                .Union(instrumentsInRoubles)
                                .GroupBy(p => p.Key)
                                .ToDictionary(p => p.Key, p => p.Sum(s => s.Value))
                                .Select(currencyGroup => new AccountCurrencyData(currencyGroup.Key, currencyGroup.Value))
                                .ToArray();

            var currencyService = DependencyService.Resolve<ICurrencyService>();
            var mergedWithPlanned = await currencyService.MergeCurrenciesDataAsync(accountId, dataByClient);
            return mergedWithPlanned;
        }

        /// <summary>
        /// Объединение данных полученных из внешнего источника с локальными данными.
        /// </summary>
        /// <param name="externalAccounts">Внешние данные по счетам.</param>
        /// <returns>Данные по счетам.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task<Account[]> MergeAccountDataAsync(IReadOnlyCollection<Account> externalAccounts)
        {
            if (externalAccounts == null)
            {
                throw new ArgumentNullException(nameof(externalAccounts), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var accounts = await dataAccessService.GetAccountDataAsync();
            var result = (accounts ?? Array.Empty<AccountData>()).ToList();
            foreach (var externalAccountId in externalAccounts.Select(a => a.ID))
            {
                var account = result.Find(x => x.Number == externalAccountId);
                if (account == null)
                {
                    account = new AccountData(externalAccountId);
                    result.Add(account);
                }
            }

            await dataAccessService.SaveAccountDataAsync(result.ToArray());
            return externalAccounts.ToArray();
        }
    }
}

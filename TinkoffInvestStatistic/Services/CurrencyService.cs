using Domain;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class CurrencyService : ICurrencyService
    {
        /// <inheritdoc/>
        public Task SavePlanPercents(string accountNumber, CurrencyData[] currenciesData)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученный номер счета не может быть неопределенным.");
            }

            if (currenciesData == null || currenciesData.Length == 0)
            {
                throw new ArgumentNullException(nameof(currenciesData), "Полученные данные о валютах не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.SaveCurrenciesDataAsync(accountNumber, currenciesData);
        }

        /// <inheritdoc/>
        public async Task<AccountCurrencyData[]> MergeCurrenciesDataAsync(string accountNumber, AccountCurrencyData[] currencies)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученный номер счета не может быть неопределенным.");
            }

            if (currencies == null || !currencies.Any())
            {
                return Array.Empty<AccountCurrencyData>();
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var currenciesEntities = await dataAccessService.GetCurrenciesDataAsync(accountNumber);

            var result = new List<AccountCurrencyData>();
            foreach (var currency in currencies)
            {
                var currencyEntity = currenciesEntities.FirstOrDefault(i => i.Currency == currency.Currency);
                result.Add(new AccountCurrencyData(currency.Currency, currencyEntity?.PlanPercent ?? 0, currency.Sum));
            }

            return result.ToArray();
        }
    }
}

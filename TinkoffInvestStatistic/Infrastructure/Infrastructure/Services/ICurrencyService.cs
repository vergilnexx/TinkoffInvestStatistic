using Domain;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис по работе с валютами.
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Сохраняет данные по валютам.
        /// </summary>
        /// <param name="accountNumber">Идентификатор счета.</param>
        /// <param name="currenciesData">Данные по валютам.</param>
        /// <returns></returns>
        Task SavePlanPercents(string accountNumber, CurrencyData[] currenciesData);

        /// <summary>
        /// Возвращает заполненные данные по инструментам.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="currencies">Данные по валюте.</param>
        /// <returns>Заполненные данные по инструментам</returns>
        Task<AccountCurrencyData[]> MergeCurrenciesDataAsync(string accountNumber, AccountCurrencyData[] currencies);
    }
}

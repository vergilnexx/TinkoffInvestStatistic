using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Infrastructure.Clients
{
    /// <summary>
    /// Клиент работы с APIброкером.
    /// </summary>
    public interface IBankBrokerApiClient
    {
        /// <summary>
        /// Возвращает счета.
        /// </summary>
        /// <returns>Список счетов только для чтения.</returns>
        public Task<IReadOnlyCollection<Account>> GetAccountsAsync();

        /// <summary>
        /// Возвращает полные данные по счету.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <returns>Полные данные по счету.</returns>
        public Task<Portfolio> GetAccountsFullDataAsync(string accountId);

        /// <summary>
        /// Возвращает данные по валютам.
        /// </summary>
        /// <returns>Список данных по валютам.</returns>
        public Task<IReadOnlyCollection<CurrencyMoney>> GetCurrenciesAsync();

        /// <summary>
        /// Возвращает фиатные позиции по счету.
        /// </summary>
        /// <param name="accountId">Номер счета.</param>
        /// <returns>Список фиатных позиций только для чтения.</returns>
        public Task<IReadOnlyCollection<CurrencyMoney>> GetFiatPositionsAsync(string accountId);

        /// <summary>
        /// Возвращает позиции по тикеру.
        /// </summary>
        /// <param name="ticker">Тикер.</param>
        /// <returns>Список позиций</returns>
        public Task<IReadOnlyCollection<Position>> FindPositionsAsync(string ticker);
    }
}

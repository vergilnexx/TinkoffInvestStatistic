using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;

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
        /// Возвращает позицию по уникальному фин. идентификатору.
        /// </summary>
        /// <param name="figi">Уникальный фин.идентификатор.</param>
        /// <param name="positionType">Тип позиции.</param>
        /// <returns>Позиция по уникальному фин. идентификатору.</returns>
        public Task<Position> FindPositionByFigiAsync(string figi, PositionType positionType);
    }
}

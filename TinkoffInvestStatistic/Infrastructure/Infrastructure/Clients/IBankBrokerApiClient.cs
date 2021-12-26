using Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Возвращает позиции по счету.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <returns>Список позиций только для чтения.</returns>
        public Task<IReadOnlyCollection<Position>> GetPositionsAsync(string accountId);
    }
}

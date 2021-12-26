using Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис статистики.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Возвращает список счетов.
        /// </summary>
        /// <returns>Список счетов только для чтения.</returns>
        public Task<IReadOnlyCollection<Account>> GetAccountsAsync();
    }
}

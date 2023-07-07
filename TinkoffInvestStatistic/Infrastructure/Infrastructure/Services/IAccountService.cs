using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис счетов.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Возвращает список счетов.
        /// </summary>
        /// <returns>Список счетов только для чтения.</returns>
        public Task<IReadOnlyCollection<Account>> GetAccountsAsync();

        /// <summary>
        /// Возвращает данные по счету в разрезе валют.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <returns>Данные по счету в разрезе валют.</returns>
        public Task<IReadOnlyCollection<AccountCurrencyData>> GetAccountDataByCurrenciesTypes(string accountId);
    }
}

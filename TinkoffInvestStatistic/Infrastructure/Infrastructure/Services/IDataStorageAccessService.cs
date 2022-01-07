using Domain;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Интерфейс работы с данными.
    /// </summary>
    public interface IDataStorageAccessService
    {
        /// <summary>
        /// Возвращает данные о счетах.
        /// </summary>
        /// <returns>Данные о счетах.</returns>
        Task<AccountData[]> GetAccountDataAsync();

        /// <summary>
        /// Сохранение данных о счетах.
        /// </summary>
        /// <param name="data">Данные о счетах.</param>
        Task SaveAccountDataAsync(AccountData[] data);
    }
}

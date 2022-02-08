using Domain;
using System.Threading.Tasks;

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
        /// <param name="accountId">Идентификатор счета.</param>
        /// <param name="currencyDatas">Данные по валютам.</param>
        /// <returns></returns>
        Task SavePlanPercents(string accountId, CurrencyData[] currencyDatas);
    }
}

using Domain;
using Infrastructure.Services;
using System.Threading.Tasks;

namespace Services
{
    /// <inheritdoc/>
    public class CurrencyService : ICurrencyService
    {
        /// <inheritdoc/>
        public Task SavePlanPercents(string accountNumber, CurrencyData[] data)
        {
            return DataStorageService.Instance.SaveCurrenciesData(accountNumber, data);
        }
    }
}

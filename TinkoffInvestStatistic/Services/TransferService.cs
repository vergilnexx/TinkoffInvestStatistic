using Infrastructure.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Services
{
    /// <inheritdoc/>
    public class TransferService : ITransferService
    {
        /// <summary>
        /// Список джоступных брокеров.
        /// </summary>
        public readonly static IReadOnlyCollection<string> BrokersName = new[]
        {
            "БКС",
            "ВТБ",
            "Тинькофф"
        };

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<TransferBroker>> GetListAsync(CancellationToken cancellation)
        {
            return DataStorageService.Instance.GetTransfersAsync(cancellation);
        }

        /// <inheritdoc/>
        public Task SaveAsync(string brokerName, IReadOnlyCollection<TransferBrokerAccount> amounts, CancellationToken cancellation)
        {
            return DataStorageService.Instance.SaveTransfersAsync(brokerName, amounts, cancellation);
        }

        /// <inheritdoc/>
        public Task AddTransferBrokerAccountAsync(string brokerName, string name, CancellationToken cancellation)
        {
            return DataStorageService.Instance.AddTransferBrokerAccountAsync(brokerName, name, cancellation);
        }
    }
}

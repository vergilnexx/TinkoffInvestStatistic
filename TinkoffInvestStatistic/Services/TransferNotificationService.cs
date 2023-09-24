using Domain;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Services
{
    /// <inheritdoc/>
    public class TransferNotificationService : ITransferNotificationService
    {
        /// <inheritdoc/>
        public Task<int> AddAsync(TransferNotificationDto data, CancellationToken cancellation)
        {
            return DataStorageService.Instance.AddTransferNotificationAsync(data, cancellation);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(int id, CancellationToken cancellation)
        {
            return DataStorageService.Instance.DeleteTransferNotificationAsync(id, cancellation);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<TransferNotificationDto>> GetListAsync(CancellationToken cancellation)
        {
            return DataStorageService.Instance.GetTransferNotificationsAsync(cancellation);
        }

        /// <inheritdoc/>
        public Task SaveAsync(IReadOnlyCollection<TransferNotificationDto> notifications, CancellationToken cancellation)
        {
            return DataStorageService.Instance.SaveTransferNotificationsAsync(notifications, cancellation);
        }
    }
}

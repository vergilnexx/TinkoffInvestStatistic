using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class TransferNotificationService : ITransferNotificationService
    {
        /// <inheritdoc/>
        public async Task<int> AddAsync(TransferNotificationDto data, CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return await dataAccessService.AddTransferNotificationAsync(data, cancellation);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id, CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.DeleteTransferNotificationAsync(id, cancellation);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TransferNotificationDto>> GetListAsync(CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var notifications = await dataAccessService.GetTransferNotificationsAsync(cancellation);
            return notifications
                    .Select(n => new TransferNotificationDto(n.Id, n.StartDate, n.PeriodType))
                    .ToArray();
        }

        /// <inheritdoc/>
        public async Task SaveAsync(IReadOnlyCollection<TransferNotificationDto> notifications, CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.SaveTransferNotificationsAsync(notifications, cancellation);
        }
    }
}

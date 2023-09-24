using System.Threading.Tasks;
using System.Threading;
using TinkoffInvestStatistic.Contracts;

namespace TinkoffInvestStatistic.Service
{
    /// <summary>
    /// Сервис уведомлений.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Добавляет запрос на уведомление.
        /// </summary>
        /// <param name="notificationId">Идентификатор уведомления.</param>
        /// <param name="data">Данные.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task AddRequestAsync(int notificationId, TransferNotificationDto data, CancellationToken cancellation);

        /// <summary>
        /// Изменяет уведомление.
        /// </summary>
        /// <param name="notificationId">Идентификатор.</param>
        /// <param name="notification">Данные уведомления.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task ChangeAsync(int notificationId, TransferNotificationDto notification, CancellationToken cancellation);

        /// <summary>
        /// Удаляет уведомление.
        /// </summary>
        /// <param name="notificationId">Идентификатор уведомления.</param>
        void Delete(int notificationId);
    }
}

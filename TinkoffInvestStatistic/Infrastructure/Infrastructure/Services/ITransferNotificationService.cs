using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис для работы с уведомлениями о зачислениях.
    /// </summary>
    public interface ITransferNotificationService
    {
        /// <summary>
        /// Добавляет новое уведомление о необходимости зачисления средств.
        /// </summary>
        /// <param name="data">Данные о времени и периоде зачисления.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Идентификатор уведомления.</returns>
        Task<int> AddAsync(TransferNotificationDto data, CancellationToken cancellation);

        /// <summary>
        /// Удаляет уведомление о необходимости зачисления средств.
        /// </summary>
        /// <param name="id">Идентификатор уведомления.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task DeleteAsync(int id, CancellationToken cancellation);

        /// <summary>
        /// Возвращает список уведомлений.
        /// </summary>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список уведомлений о необходимости зачисления средств.</returns>
        Task<IReadOnlyCollection<TransferNotificationDto>> GetListAsync(CancellationToken cancellation);
        
        /// <summary>
        /// Сохраняет данные уведомлений.
        /// </summary>
        /// <param name="notifications">Данные уведомлений.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task SaveAsync(IReadOnlyCollection<TransferNotificationDto> notifications, CancellationToken cancellation);
    }
}

using System.Threading.Tasks;

namespace TinkoffInvestStatistic.Service
{
    /// <summary>
    /// Сервис уведомлений UI
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Показывает уведомление на экране.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        Task ShowAsync(string message);
    }
}

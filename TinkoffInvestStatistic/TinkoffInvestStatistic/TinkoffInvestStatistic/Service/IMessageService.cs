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

        /// <summary>
        /// Показывает диалог на экране с возможностью ввода текста.
        /// </summary>
        /// <param name="title">Заголовок.</param>
        /// <param name="message">Сообщение.</param>
        Task<string> ShowPromptAsync(string title, string message);
    }
}

using Xamarin.Forms;

namespace TinkoffInvestStatistic.Service
{
    /// <summary>
    /// Сервис по скрытию/отображения денег на страницах.
    /// </summary>
    public interface IHideShowMoneyService
    {
        /// <summary>
        /// Возвращает название файла иконки.
        /// </summary>
        /// <returns></returns>
        string GetIconFileName();

        /// <summary>
        /// Возвращает признак видимости данных.
        /// </summary>
        /// <returns>Признак видимости данных.</returns>
        bool IsShow();

        /// <summary>
        /// Устанавливает признак видимости данных.
        /// </summary>
        /// <param name="isShow">Признак видимости данных.</param>
        void SetShow(bool isShow);
    }
}

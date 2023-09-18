using System.ComponentModel;

namespace TinkoffInvestStatistic.Contracts.Enums
{
    /// <summary>
    /// Периодичность уведомлений о зачислении.
    /// </summary>
    public enum TransferNotificationPeriodType
    {
        /// <summary>
        /// Неопределенно.
        /// </summary>
        None = 0,

        /// <summary>
        /// Неделя.
        /// </summary>
        [Description("Каждую неделю")]
        Week = 20,

        /// <summary>
        /// Месяц.
        /// </summary>
        [Description("Каждый месяц")]
        Month = 30,

        /// <summary>
        /// Квартер.
        /// </summary>
        [Description("Каждый квартер")]
        Quarter = 40,

        /// <summary>
        /// Год.
        /// </summary>
        [Description("Каждый год")]
        Year = 50,
    }
}

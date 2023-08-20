using System;

namespace TinkoffInvestStatistic.Contracts.Enums
{
    /// <summary>
    /// Категории для экспорта.
    /// </summary>
    [Flags]
    public enum ExportCategories
    {
        /// <summary>
        /// Неопределенно.
        /// </summary>
        None = 0,

        /// <summary>
        /// Настройки.
        /// </summary>
        Settings = 1,

        /// <summary>
        /// Данные.
        /// </summary>
        Data = 2,
    }
}

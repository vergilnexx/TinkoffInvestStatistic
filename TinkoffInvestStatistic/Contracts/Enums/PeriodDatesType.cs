﻿using System.ComponentModel;

namespace TinkoffInvestStatistic.Contracts.Enums
{
    /// <summary>
    /// Периодичность дат.
    /// </summary>
    public enum PeriodDatesType
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
        /// Квартал.
        /// </summary>
        [Description("Каждый квартал")]
        Quarter = 40,

        /// <summary>
        /// Год.
        /// </summary>
        [Description("Каждый год")]
        Year = 50,
    }
}

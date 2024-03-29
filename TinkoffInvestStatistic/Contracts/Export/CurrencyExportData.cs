﻿using System;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts.Export
{
    /// <summary>
    /// Данные экспорта валют.
    /// </summary>
    public class CurrencyExportData
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public CurrencyExportData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        public CurrencyExportData(Currency currency) : this()
        {
            Currency = currency;
        }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal? PlanPercent { get; set; }
    }
}

﻿using Infrastructure.Helpers;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Utility;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель о валюте.
    /// </summary>
    public class CurrencyTypeModel
    {
        /// <summary>
        /// Наименоавние валюты.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public decimal CurrentSum { get; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public string CurrentSumText { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public string PlanPercent { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercentValue => NumericUtility.TryParse(PlanPercent);

        /// <summary>
        /// Текущий процент от суммы по всем инструментам.
        /// </summary>
        public decimal CurrentPercent { get; }

        /// <summary>
        /// Цвет процента.
        /// </summary>
        public Color CurrentPercentColor => DifferencePercentUtility.GetPercentColor(CurrentPercent, PlanPercentValue);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <param name="currentSum">Сумма.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        /// <param name="currentPercent">Текущий процент.</param>
        public CurrencyTypeModel(Currency currency, decimal currentSum, string planPercent, decimal currentPercent)
        {
            Name = currency.GetDescription();
            Currency = currency;
            CurrentSum = currentSum;
            PlanPercent = planPercent;
            CurrentPercent = currentPercent;
        }
    }
}

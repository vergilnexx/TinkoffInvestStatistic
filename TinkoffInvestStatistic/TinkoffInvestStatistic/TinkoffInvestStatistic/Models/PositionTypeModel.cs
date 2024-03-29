﻿using Infrastructure.Helpers;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Utility;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель типа инструмента.
    /// </summary>
    public class PositionTypeModel
    {
        /// <summary>
        /// Наименоавние типа.
        /// </summary>
        public string TypeName => Type.GetDescription();

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; }

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
        public decimal CurrentPercent { get; set; }

        /// <summary>
        /// Цвет процента.
        /// </summary>
        public Color CurrentPercentColor => DifferencePercentUtility.GetPercentColor(CurrentPercent, PlanPercentValue);

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public decimal CurrentSum { get; set; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public string CurrentSumText { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип./param>
        public PositionTypeModel(PositionType type)
        {
            Type = type;
        }
    }
}

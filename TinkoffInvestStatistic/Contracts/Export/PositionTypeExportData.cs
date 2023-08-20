using System.Collections.Generic;
using System;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts.Export
{
    /// <summary>
    /// Данные типов позиций для экспорта.
    /// </summary>
    public class PositionTypeExportData
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PositionTypeExportData()
        {
            Positions = Array.Empty<PositionExportData>();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип позиции.</param>
        public PositionTypeExportData(PositionType type) : this() 
        {
            Type = type;
        }

        /// <summary>
        /// Тип инструмента.
        /// </summary>
        public PositionType Type { get; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Данные позиций для экспорта.
        /// </summary>
        public PositionExportData[] Positions { get; set; }
    }
}

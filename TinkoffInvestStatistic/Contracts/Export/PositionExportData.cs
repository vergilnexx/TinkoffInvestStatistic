using System;

namespace TinkoffInvestStatistic.Contracts.Export
{
    /// <summary>
    /// Даннае для экспорта позиций.
    /// </summary>
    public class PositionExportData
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PositionExportData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figi">Глобальный идентификатор финансового инструмента.</param>
        public PositionExportData(string figi) : this()
        {
            Figi = figi;
        }

        /// <summary>
        /// Глобальный идентификатор финансового инструмента
        /// </summary>
        public string? Figi { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }
    }
}

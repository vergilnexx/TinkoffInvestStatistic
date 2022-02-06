﻿using Contracts.Enums;

namespace Contracts
{
    /// <summary>
    /// Данные об инструменте.
    /// </summary>
    public class Instrument
    {
        /// <summary>
        /// Тип инструмента.
        /// </summary>
        public PositionType Type { get; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип инструмента.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        public Instrument(PositionType type, decimal planPercent)
        {
            Type = type;
            PlanPercent = planPercent;
        }
    }
}

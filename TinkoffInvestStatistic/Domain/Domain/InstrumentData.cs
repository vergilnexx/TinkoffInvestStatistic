﻿using Contracts.Enums;

namespace Domain
{
    /// <summary>
    /// Данные об инструменте.
    /// </summary>
    public class InstrumentData
    {
        /// <summary>
        /// Тип инструмента.
        /// </summary>
        public PositionType Type { get; private set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип инструмента.</param>
        public InstrumentData(PositionType type)
        {
            Type = type;
        }
    }
}
using Contracts.Enums;

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
        public PositionType Type { get; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип инструмента.</param>
        public InstrumentData(PositionType type)
        {
            Type = type;
            PlanPercent = 0;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип инструмента.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        public InstrumentData(PositionType type, decimal planPercent)
        {
            Type = type;
            PlanPercent = planPercent;
        }
    }
}

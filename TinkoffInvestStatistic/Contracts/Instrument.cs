using Contracts.Enums;

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
        public Instrument(PositionType type)
        {
            Type = type;
        }
    }
}

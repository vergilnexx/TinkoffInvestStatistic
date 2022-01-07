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
        public PositionType Type { get; private set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }
    }
}

using Contracts.Enums;

namespace Domain
{
    /// <summary>
    /// Данные о позициях.
    /// </summary>
    public class PositionData
    {
        /// <summary>
        /// Глобальный идентификатор финансового инструмента
        /// </summary>
        public string Figi { get; private set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; private set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="type">Тип позиции.</param>
        public PositionData(string figi, PositionType type)
        {
            Figi = figi;
            Type = type;
        }
    }
}

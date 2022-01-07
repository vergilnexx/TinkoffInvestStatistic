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
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }
    }
}

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Элемент статистики.
    /// </summary>
    public class StatisticItem
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Сумма в рублях.
        /// </summary>
        public decimal SumInRub { get; set; }

        /// <summary>
        /// Процент.
        /// </summary>
        public decimal Percent { get; set; }
    }
}

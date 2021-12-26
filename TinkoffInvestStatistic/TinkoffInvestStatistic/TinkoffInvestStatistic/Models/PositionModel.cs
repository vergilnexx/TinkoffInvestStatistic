namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PositionModel
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тикер.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Баланс.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Заблокировано.
        /// </summary>
        public decimal Blocked { get; set; }

        /// <summary>
        /// Количество лотов.
        /// </summary>
        public int Lots { get; set; }
    }
}

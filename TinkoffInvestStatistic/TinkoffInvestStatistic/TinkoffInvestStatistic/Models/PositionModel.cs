using Contracts.Enums;

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
        public decimal PositionCount { get; set; }

        /// <summary>
        /// Заблокировано.
        /// </summary>
        public decimal Blocked { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Сумма в валюте.
        /// </summary>
        public decimal SumInCurrency { get; set; }
    }
}

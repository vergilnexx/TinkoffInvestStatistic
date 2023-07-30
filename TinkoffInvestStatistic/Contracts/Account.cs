using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Счет.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Наименование счета.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип счета.
        /// </summary>
        public AccountType Type { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public decimal Sum { get; set; }
    }
}

using System.ComponentModel;

namespace TinkoffInvestStatistic.Contracts.Enums
{
    /// <summary>
    /// Тип счета
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// ИИС
        /// </summary>
        [Description("ИИС")]
        Iis = 1,

        /// <summary>
        /// Брокерский счет
        /// </summary>
        [Description("Брокерский счет")]
        BrokerAccount = 2,

        /// <summary>
        /// Инвесткопилка
        /// </summary>
        [Description("Инвесткопилка")]
        InvestBox = 3
    }
}

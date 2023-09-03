namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Данные зачислений по счету брокера.
    /// </summary>
    public class TransferBrokerAccount
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Наименование счета.</param>
        /// <param name="sum">Сумма зачислений.</param>
        public TransferBrokerAccount(string name, decimal sum)
        {
            Name = name;
            Sum = sum;
        }

        /// <summary>
        /// Наименование счета.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Сумма зачислений.
        /// </summary>
        public decimal Sum { get; set; }
    }
}

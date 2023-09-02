namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель данных зачислений по брокеру.
    /// </summary>
    public class TransferBrokerModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="brokerName">Наименование брокера.</param>
        public TransferBrokerModel(string brokerName, decimal sum)
        {
            BrokerName = brokerName ?? string.Empty;
            Sum = sum;
        }

        /// <summary>
        /// Наименование брокера.
        /// </summary>
        public string BrokerName { get; }

        /// <summary>
        /// Сумма зачислений.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Зачислить.
        /// </summary>
        public decimal Amount { get; set; }
    }
}

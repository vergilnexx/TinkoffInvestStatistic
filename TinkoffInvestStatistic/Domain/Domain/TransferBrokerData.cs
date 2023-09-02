using SQLite;

namespace Domain
{
    /// <summary>
    /// Данные о зачислениях по брокерам.
    /// </summary>
    public class TransferBrokerData
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Наименование брокера.
        /// </summary>
        public string BrokerName { get; set; }

        /// <summary>
        /// Сумма зачислений.
        /// </summary>
        public decimal Sum { get; set; }
    }
}

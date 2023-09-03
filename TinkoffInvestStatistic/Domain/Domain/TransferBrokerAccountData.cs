using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Domain
{
    /// <summary>
    /// Данные о зачислениях по счетам брокера.
    /// </summary>
    public class TransferBrokerAccountData
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор брокера.
        /// </summary>
        [ForeignKey(typeof(TransferBrokerData))]
        public int BrokerId { get; set; }

        /// <summary>
        /// Наименование счета.
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Сумма зачислений.
        /// </summary>
        public decimal Sum { get; set; }
    }
}

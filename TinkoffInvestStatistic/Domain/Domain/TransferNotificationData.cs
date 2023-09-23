using SQLite;
using System;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Domain
{
    /// <summary>
    /// Данные уведомлений о необходимости заччисления средств.
    /// </summary>
    public class TransferNotificationData
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Дата начала отсчета.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Периодичность уведомления.
        /// </summary>
        public PeriodDatesType PeriodType { get; set; }
    }
}

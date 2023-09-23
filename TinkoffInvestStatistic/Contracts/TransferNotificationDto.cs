using System;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Данные уведомления о необходимости зачисления средств.
    /// </summary>
    public class TransferNotificationDto
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="periodType">Периодичность уведомления.</param>
        public TransferNotificationDto(DateTime startDate, PeriodDatesType periodType)
        {
            StartDate = startDate;
            PeriodType = periodType;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="periodType">Периодичность уведомления.</param>
        public TransferNotificationDto(int id, DateTime startDate, PeriodDatesType periodType) : this(startDate, periodType)
        {
            Id = id;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата начала.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Периодичность уведомления.
        /// </summary>
        public PeriodDatesType PeriodType { get; set; }
    }
}

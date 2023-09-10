using System;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Данные уведомления о необходимости зачисления.
    /// </summary>
    public class TransferNotificationModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="periodType">Периодичность уведомления.</param>
        public TransferNotificationModel(DateTime startDate, string periodType)
        {
            StartDate = startDate.Date;
            Time = startDate.TimeOfDay;
            PeriodTypeText = periodType;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="periodType">Периодичность уведомления.</param>
        public TransferNotificationModel(int id, DateTime startDate, string periodType) : this(startDate, periodType)
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
        /// Время начала отсчета.
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Периодичность уведомления.
        /// </summary>
        public string PeriodTypeText { get; set; }
    }
}

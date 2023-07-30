using TinkoffInvestStatistic.Contracts.Enums;
using SQLite;

namespace Domain
{
    /// <summary>
    /// Данные об инструменте.
    /// </summary>
    public class PositionTypeData
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Номер счета.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Тип инструмента.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PositionTypeData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип инструмента.</param>
        public PositionTypeData(PositionType type)
        {
            Type = type;
            PlanPercent = 0;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="type">Тип инструмента.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        public PositionTypeData(string accountNumber, PositionType type, decimal planPercent)
        {
            AccountNumber = accountNumber;
            Type = type;
            PlanPercent = planPercent;
        }
    }
}

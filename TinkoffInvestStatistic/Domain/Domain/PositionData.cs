using Contracts.Enums;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Domain
{
    /// <summary>
    /// Данные о позициях.
    /// </summary>
    public class PositionData
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
        /// Глобальный идентификатор финансового инструмента
        /// </summary>
        public string Figi { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PositionData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="type">Тип позиции.</param>
        public PositionData(string accountNumber, string figi, PositionType type)
        {
            AccountNumber = accountNumber;
            Figi = figi;
            Type = type;
        }
    }
}

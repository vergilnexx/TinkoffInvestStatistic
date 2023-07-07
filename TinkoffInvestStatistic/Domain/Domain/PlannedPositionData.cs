using TinkoffInvestStatistic.Contracts.Enums;
using SQLite;

namespace Domain
{
    /// <summary>
    /// Планируемая для покупки позиция.
    /// </summary>
    public class PlannedPositionData
    {
        /// <summary>
        /// Глобальный идентификатор финансового инструмента
        /// </summary>
        [PrimaryKey]
        public string Figi { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Номер счета.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PlannedPositionData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="type">Тип инструмента.</param>
        /// <param name="figi">Глобальный идентификатор финансового инструмента.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="ticker">Тикер.</param>
        public PlannedPositionData(string accountNumber, string figi, PositionType type, string name, string ticker)
        {
            this.AccountNumber = accountNumber;
            Figi = figi;
            Type = type;
            Name = name;
            Ticker = ticker;
        }
    }
}

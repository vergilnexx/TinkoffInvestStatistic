using Contracts.Enums;

namespace Contracts
{
    /// <summary>
    /// Позиция.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Financial Instrument Global Identifier.
        /// </summary>
        public string Figi { get; set; } = string.Empty;

        /// <summary>
        /// Тикер.
        /// </summary>
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// Количество позиций.
        /// </summary>
        public decimal PositionCount { get; set; }

        /// <summary>
        /// Заблокировано.
        /// </summary>
        public decimal Blocked { get; set; }

        /// <summary>
        /// Текущее изменение цены.
        /// </summary>
        public CurrencyMoney? ExpectedYield { get; set; }

        /// <summary>
        /// Цена покупки.
        /// </summary>
        public CurrencyMoney? AveragePositionPrice { get; set; }

        /// <summary>
        /// Цеа покупки без налогов.
        /// </summary>
        public CurrencyMoney? AveragePositionPriceNoNkd { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="type">Тип инструмента.</param>
        public Position(string figi, PositionType type)
        {
            Figi = figi;
            Type = type;
        }
    }
}

using Contracts.Enums;
using System;

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
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Financial Instrument Global Identifier.
        /// </summary>
        public string Figi { get; set; } = String.Empty;

        /// <summary>
        /// Тикер.
        /// </summary>
        public string Ticker { get; set; } = String.Empty;

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
    }
}

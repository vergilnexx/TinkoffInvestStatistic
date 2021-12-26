using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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
        public string Name { get; set; }

        /// <summary>
        /// Financial Instrument Global Identifier.
        /// </summary>
        public string Figi { get; set; }

        /// <summary>
        /// Тикер.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// Баланс.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Заблокировано.
        /// </summary>
        public decimal Blocked { get; set; }

        public CurrencyMoney ExpectedYield { get; set; }

        /// <summary>
        /// Количество лотов.
        /// </summary>
        public int Lots { get; set; }

        public CurrencyMoney AveragePositionPrice { get; set; }

        public CurrencyMoney AveragePositionPriceNoNkd { get; set; }
    }
}

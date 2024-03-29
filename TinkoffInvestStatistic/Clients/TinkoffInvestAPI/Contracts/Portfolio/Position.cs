﻿using Newtonsoft.Json;
using TinkoffInvest.Contracts.Common;
using TinkoffInvest.Contracts.Enums;

namespace TinkoffInvest.Contracts.Portfolio
{
    /// <summary>
    /// Данные о позиции.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Глобальный идентификатор финансового инструмента.
        /// </summary>
        [JsonProperty(PropertyName = "figi")]
        public string Figi { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public Numeric Quantity { get; set; }

        /// <summary>
        /// Текущая цена в валюте.
        /// </summary>
        [JsonProperty(PropertyName = "currentPrice")]
        public CurrencyNumeric CurrencyCurrentPrice { get; set; }

        /// <summary>
        /// Тип финансового инструмента.
        /// </summary>
        [JsonProperty(PropertyName = "instrumentType")]
        public InstrumentType InstrumentType { get; set; }

        /// <summary>
        /// Средняя цена.
        /// </summary>
        [JsonProperty(PropertyName = "averagePositionPrice")]
        public CurrencyNumeric AveragePositionPrice { get; set; }

        /// <summary>
        /// Признак, что позиция заблокирована.
        /// </summary>
        [JsonProperty(PropertyName = "blocked")]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Текущая рассчитанная доходность позиции.
        /// </summary>
        [JsonProperty(PropertyName = "expectedYield")]
        public Numeric ExpectedYield { get; set; }
    }
}

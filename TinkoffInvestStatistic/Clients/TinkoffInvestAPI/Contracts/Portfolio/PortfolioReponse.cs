using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TinkoffInvest.Contracts.Common;

namespace TinkoffInvest.Contracts.Portfolio
{
    /// <summary>
    /// Ответ на получение данных о портфеле.
    /// </summary>
    public class PortfolioReponse
    {
        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// Полная стоимость портфеля.
        /// </summary>
        [JsonProperty(PropertyName = "totalAmountPortfolio")]
        public CurrencyNumeric TotalAmount { get; set; }

        /// <summary>
        /// Позиции.
        /// </summary>
        [JsonProperty(PropertyName = "positions")]
        public IReadOnlyCollection<Position> Positions { get; set; } = Array.Empty<Position>();
    }
}

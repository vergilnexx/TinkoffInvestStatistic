using Newtonsoft.Json;

namespace TinkoffInvest.Contracts.Instruments
{
    /// <summary>
    /// Данные инструмента.
    /// </summary>
    public class Instrument
    {
        /// <summary>
        /// Глобальный идентификатор финансового инструмента.
        /// </summary>
        [JsonProperty(PropertyName = "figi")]
        public string Figi { get; set; }

        /// <summary>
        /// Тикер.
        /// </summary>
        [JsonProperty(PropertyName = "ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Сектор.
        /// </summary>
        [JsonProperty(PropertyName = "sector")]
        public string Sector { get; set; }

        /// <summary>
        /// Страна.
        /// </summary>
        [JsonProperty(PropertyName = "countryOfRiskName")]
        public string Country { get; set; }

    }
}

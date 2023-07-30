using Newtonsoft.Json;

namespace TinkoffInvest.Contracts.Instruments
{
    /// <summary>
    /// Данные ответа по запросу инструмента.
    /// </summary>
    public class InstrumentResponse
    {
        /// <summary>
        /// Данные инструмента.
        /// </summary>
        [JsonProperty(PropertyName = "instrument")]
        public Instrument Instrument { get; set; }
    }
}

using Newtonsoft.Json;

namespace TinkoffInvest.Contracts.Common
{
    /// <summary>
    /// Числовое представление.
    /// </summary>
    public class Numeric
    {
        /// <summary>
        /// Целая часть.
        /// </summary>
        [JsonProperty(PropertyName = "units")]
        public string IntegerPart { get; set; }

        /// <summary>
        /// Дробная часть.
        /// </summary>
        [JsonProperty(PropertyName = "nano")]
        public long FractionalPart { get; set; }
    }
}

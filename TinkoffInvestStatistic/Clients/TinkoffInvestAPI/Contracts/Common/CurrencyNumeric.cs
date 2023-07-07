using TinkoffInvest.Contracts.Enums;

namespace TinkoffInvest.Contracts.Common
{
    /// <summary>
    /// Числовое представление с валютой.
    /// </summary>
    public class CurrencyNumeric : Numeric
    {
        /// <summary>
        /// Валюта.
        /// </summary>
        public CurrencyType Currency { get; set; }
    }
}

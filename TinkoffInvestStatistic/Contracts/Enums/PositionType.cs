using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Тип позиции.
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// Акция.
        /// </summary>
        [Description("Акция")]
        Stock,

        /// <summary>
        /// Валюта.
        /// </summary>
        [Description("Валюта")]
        Currency,

        /// <summary>
        /// Облигация.
        /// </summary>
        [Description("Облигация")]
        Bond,

        /// <summary>
        /// Фонд.
        /// </summary>
        [Description("Фонд")]
        Etf
    }
}

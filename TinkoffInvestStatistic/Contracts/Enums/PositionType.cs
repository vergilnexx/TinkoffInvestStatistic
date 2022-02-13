using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Тип позиции.
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// Акции.
        /// </summary>
        [Description("Акции")]
        Stock,

        /// <summary>
        /// Валюта.
        /// </summary>
        [Description("Валюта")]
        Currency,

        /// <summary>
        /// Облигации.
        /// </summary>
        [Description("Облигации")]
        Bond,

        /// <summary>
        /// Фонды.
        /// </summary>
        [Description("Фонды")]
        Etf
    }
}

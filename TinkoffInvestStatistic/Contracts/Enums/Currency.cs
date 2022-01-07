using Contracts.Attributes;
using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Валюты.
    /// </summary>
    public enum Currency
    {
        /// <summary>
        /// Рубль.
        /// </summary>
        [Description("RUB")]
        Rub,

        /// <summary>
        /// Доллар.
        /// </summary>
        [Description("USD")]
        [Figi("BBG0013HGFT4")]
        Usd,

        /// <summary>
        /// Еро.
        /// </summary>
        [Description("EUR")]
        [Figi("BBG0013HJJ31")]
        Eur,

        /// <summary>
        /// Фунт стерлингов
        /// </summary>
        [Description("Фунт стерлингов")]
        Gbp,

        /// <summary>
        /// Гонконгский доллар
        /// </summary>
        [Description("Гонконгский доллар")]
        Hkd,

        /// <summary>
        /// Швейцарский франк
        /// </summary>
        [Description("Швейцарский франк")]
        Chf,

        /// <summary>
        /// Иена
        /// </summary>
        [Description("Японская иена")]
        Jpy,

        /// <summary>
        /// Китайский юань
        /// </summary>
        [Description("Китайский юань")]
        Cny,

        /// <summary>
        /// Турецкая лира
        /// </summary>
        [Description("Турецкая лира")]
        Try
    }
}

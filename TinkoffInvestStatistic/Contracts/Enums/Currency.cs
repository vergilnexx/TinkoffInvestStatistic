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
        [Figi("BBG0013HQ5F0")]
        Gbp,

        /// <summary>
        /// Гонконгский доллар
        /// </summary>
        [Description("Гонконгский доллар")]
        [Figi("BBG0013HSW87")]
        Hkd,

        /// <summary>
        /// Швейцарский франк
        /// </summary>
        [Description("Швейцарский франк")]
        [Figi("BBG0013HQ5K4")]
        Chf,

        /// <summary>
        /// Иена
        /// </summary>
        [Description("Японская иена")]
        [Figi("BBG0013HQ524")]
        Jpy,

        /// <summary>
        /// Китайский юань
        /// </summary>
        [Description("Китайский юань")]
        [Figi("BBG0013HRTL0")]
        Cny,

        /// <summary>
        /// Турецкая лира
        /// </summary>
        [Description("Турецкая лира")]
        [Figi("BBG0013J12N1")]
        Try
    }
}

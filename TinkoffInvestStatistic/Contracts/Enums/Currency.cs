using System.ComponentModel;
using TinkoffInvestStatistic.Contracts.Attributes;

namespace TinkoffInvestStatistic.Contracts.Enums
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
        /// Гонконгский доллар
        /// </summary>
        [Description("Гонконгский доллар")]
        [Figi("BBG0013HSW87")]
        Hkd,

        /// <summary>
        /// Китайский юань
        /// </summary>
        [Description("Китайский юань")]
        [Figi("BBG0013HRTL0")]
        Cny
    }
}

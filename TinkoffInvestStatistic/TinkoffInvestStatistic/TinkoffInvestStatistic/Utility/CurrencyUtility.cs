using Contracts.Enums;
using System;
using System.Globalization;

namespace TinkoffInvestStatistic.Utility
{
    public class CurrencyUtility
    {
        public static string ToCurrencyString(decimal value, Currency currency)
        {
            var culture = new CultureInfo("ru-RU");
            switch (currency)
            {
                case Currency.Rub:
                    break;
                case Currency.Usd:
                    culture.NumberFormat.CurrencySymbol = "$";
                    break;
                case Currency.Eur:
                    culture.NumberFormat.CurrencySymbol = "€";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currency));
            }

            return value.ToString("C", culture);
        }
    }
}
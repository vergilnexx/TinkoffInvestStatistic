using Infrastructure.Helpers;
using System;
using System.Globalization;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Utility
{
    /// <summary>
    /// Методы расширения для работы с числами.
    /// </summary>
    public static class NumericUtility
    {
        /// <summary>
        /// Возвращает числовую строку с учетом валюты
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <param name="currency">Валюта.</param>
        /// <returns>Числовая строка с валютой.</returns>
        /// <example>72,32 €</example>
        /// <exception cref="ArgumentOutOfRangeException">Исключение, когда неизвестная валюта</exception>
        public static string ToCurrencyString(this decimal value, Currency currency)
        {
            var culture = new CultureInfo("ru-RU");
            switch (currency)
            {
                case Currency.Rub:
                    culture.NumberFormat.CurrencySymbol = "₽";
                    break;
                case Currency.Usd:
                    culture.NumberFormat.CurrencySymbol = "$";
                    break;
                case Currency.Eur:
                    culture.NumberFormat.CurrencySymbol = "€";
                    break;
                case Currency.Hkd:
                    culture.NumberFormat.CurrencySymbol = "HK$";
                    break;
                case Currency.Cny:
                    culture.NumberFormat.CurrencySymbol = "¥";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currency));
            }

            return value.ToString("C", culture);
        }

        /// <summary>
        /// Возвращает рассчитанный процент значения от суммы.
        /// </summary>
        /// <param name="sum">Сумма.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Рассчитанный процент.</returns>
        public static decimal ToPercentage(decimal sum, decimal value)
        {
            var percentage = sum == 0 ? 0 : 100 * value / sum;
            return Math.Round(percentage, DecimalHelper.NUMERIC_DECIMALS, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Возвращает числовую строку в процентах
        /// </summary>
        /// <param name="sum">Сумма.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Числовая строка в процентах.</returns>
        /// <example>72,32 %</example>
        public static string ToPercentageString(decimal sum, decimal value)
        {
            var percentage = ToPercentage(sum, value);
            return ToPercentageString(percentage);
        }

        /// <summary>
        /// Возвращает числовую строку в процентах
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Числовая строка в процентах.</returns>
        /// <example>72,32 %</example>
        public static string ToPercentageString(this decimal value)
        {
            return value.ToString("F2", CultureInfo.InvariantCulture) + "%";
        }
    }
}
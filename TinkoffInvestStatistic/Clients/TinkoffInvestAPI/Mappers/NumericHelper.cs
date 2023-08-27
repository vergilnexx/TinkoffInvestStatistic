using Infrastructure.Helpers;
using System;
using TinkoffInvest.Contracts.Common;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Helper по работе с числовым представлением.
    /// </summary>
    public static class NumericHelper
    {
        /// <summary>
        /// Возвращает округленное сформатированное значение числового представления.
        /// </summary>
        /// <param name="numeric">Числовое представление.</param>
        /// <returns>Округленное сформатированное значение числового представления.</returns>
        public static decimal GetValue(this Numeric numeric)
        {
            if (!int.TryParse(numeric.IntegerPart, out int integerPart))
            {
                throw new ArithmeticException($"Не удалось распарсить числовое представление: {numeric.IntegerPart}");
            }

            decimal result = integerPart + (decimal)Math.Abs(numeric.FractionalPart) / 1000000000;
            return decimal.Round(result, DecimalHelper.NUMERIC_DECIMALS);
        }
    }
}

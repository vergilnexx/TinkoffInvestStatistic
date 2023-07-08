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
        /// Количество знаков после запятой
        /// </summary>
        public static int NUMERIC_DECIMALS = 2;

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

            if(!decimal.TryParse(integerPart + "." + Math.Abs(numeric.FractionalPart), out decimal result))
            {
                throw new ArithmeticException($"Не удалось преобразовать числовое представление: {integerPart}.{numeric.FractionalPart}");
            }
            
            return decimal.Round(result, NUMERIC_DECIMALS);
        }
    }
}

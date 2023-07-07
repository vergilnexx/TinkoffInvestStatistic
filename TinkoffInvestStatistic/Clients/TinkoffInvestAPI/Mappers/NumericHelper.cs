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
            => int.TryParse(numeric.IntegerPart, out int value)
                ? value + decimal.Divide(decimal.Round(numeric.FractionalPart, NUMERIC_DECIMALS), 100)
                : throw new ApplicationException(
                    $"Не удалось преобразовать числовое представление: {numeric.IntegerPart}.{numeric.FractionalPart}");
    }
}

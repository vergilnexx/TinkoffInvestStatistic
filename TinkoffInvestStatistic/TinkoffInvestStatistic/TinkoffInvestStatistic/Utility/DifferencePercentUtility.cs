using Xamarin.Forms;

namespace TinkoffInvestStatistic.Utility
{
    /// <summary>
    /// Helper для работы с разницей в процентах.
    /// </summary>
    internal class DifferencePercentUtility
    {
        /// <summary>
        /// Позволимое отклонение по разнице в процентах.
        /// </summary>
        public const decimal AllowDifferencePercent = 0.5m;

        /// <summary>
        /// Возвращает цвет для процента.
        /// </summary>
        /// <param name="currentPercentValue">Текущее значение.</param>
        /// <param name="plannedPercentValue">Планируемое значение.</param>
        /// <returns>Цвет.</returns>
        public static Color GetPercentColor(decimal currentPercentValue, decimal plannedPercentValue)
        {
            if (currentPercentValue > plannedPercentValue + AllowDifferencePercent ||
                currentPercentValue < plannedPercentValue - AllowDifferencePercent)
            {
                return Color.Red;
            }

            return Color.Green;
        }

        /// <summary>
        /// Возвращает цвет для процента без разрешенного отклонения.
        /// </summary>
        /// <param name="currentPercentValue">Текущее значение.</param>
        /// <param name="plannedPercentValue">Планируемое значение.</param>
        /// <returns>Цвет.</returns>
        public static Color GetColorPercentWithoutAllowedDifference(decimal currentPercentValue, decimal plannedPercentValue)
        {
            if (currentPercentValue != plannedPercentValue)
            {
                return Color.Red;
            }

            return Color.Green;
        }
    }
}

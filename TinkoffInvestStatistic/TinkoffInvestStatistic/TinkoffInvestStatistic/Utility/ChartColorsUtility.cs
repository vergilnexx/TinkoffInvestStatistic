using SkiaSharp;
using System;

namespace TinkoffInvestStatistic.Utility
{
    /// <summary>
    /// Работа с цветами диаграмм
    /// </summary>
    public class ChartColorsUtility
    {
        int currentIdx = 0;

        static readonly SKColor[] Colors =
        {
            SKColor.Parse("#4DC3F7"), // Blue
            SKColor.Parse("#BC71C9"), // Pinks
            SKColor.Parse("#AED57F"), // Green
            SKColor.Parse("#FFD450"), // Yellow
            SKColor.Parse("#FF78A7"), // Pinks
            SKColor.Parse("#65A7E0"), // Blue
            SKColor.Parse("#F8A34D"), // Orange
            SKColor.Parse("#4DA197"), // Green
            SKColor.Parse("#EE805D"), // Orange
            SKColor.Parse("#73B077"), // Green
        };

        public SKColor GetColor()
        {
            if (currentIdx >= Colors.Length - 1)
            {
                currentIdx = 0;
            }

            return Colors[currentIdx++];
        }
    }
}

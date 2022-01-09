﻿using SkiaSharp;
using System;

namespace TinkoffInvestStatistic.Utility
{
    /// <summary>
    /// Работа с цветами диаграмм
    /// </summary>
    public class ChartColorsUtility
    {
        public static ChartColorsUtility Instance { get; private set; }

        int currentIdx = 0;

        public ChartColorsUtility()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of ChartColors is allowed!");
            }
            else
            {
                Instance = this;
            }
        }

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
            
            //// Blue
            //SKColor.Parse("#4DC3F7"),
            //SKColor.Parse("#90CAF9"),
            //SKColor.Parse("#009688"),
            //SKColor.Parse("#80CBC4"),
            
            //// Yellow
            //SKColor.Parse("#FFEB3B"),
            //SKColor.Parse("#FFF176"),
            //SKColor.Parse("#FFD600"),
            //SKColor.Parse("#FFFF8D"),

            //// Orange
            //SKColor.Parse("#FF9800"),
            //SKColor.Parse("#FFCC80"),
            //SKColor.Parse("#FFC107"),
            //SKColor.Parse("#FFE082"),

            //// Green
            //SKColor.Parse("#4CAF50"),
            //SKColor.Parse("#A5D6A7"),
            //SKColor.Parse("#76FF03"),
            //SKColor.Parse("#CCFF90"),

            //// Reds and Pinks
            //SKColor.Parse("#F44336"),
            //SKColor.Parse("#FFCDD2"),
            //SKColor.Parse("#E91E63"),
            //SKColor.Parse("#F8BBD0"),
            
            //// Pink
            //SKColor.Parse("#9C27B0"),
            //SKColor.Parse("#E1BEE7"),
            //SKColor.Parse("#673AB7"),
            //SKColor.Parse("#D1C4E9"),
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
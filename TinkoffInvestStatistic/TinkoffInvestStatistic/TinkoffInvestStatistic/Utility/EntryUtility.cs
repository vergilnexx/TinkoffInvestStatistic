using Microcharts;
using SkiaSharp;

namespace TinkoffInvestStatistic.Utility
{
    public class EntryUtility
    {
        public static ChartEntry GetEntry(float value, SKColor color, string label, string valueLabel)
        {
            var _entry = new ChartEntry(value)
            {
                Label = label,
                ValueLabel = valueLabel,
                Color = color
            };
            return _entry;
        }
    }
}

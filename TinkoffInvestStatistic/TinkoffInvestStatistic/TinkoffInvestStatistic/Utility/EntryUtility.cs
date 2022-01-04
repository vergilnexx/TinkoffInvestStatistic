using Microcharts;
using SkiaSharp;

namespace TinkoffInvestStatistic.Utility
{
    public class EntryUtility
    {
        public static ChartEntry GetEntry(float value, SKColor color, string label, string valueLabel)
        {
            return CreateEntry(value, color, label, valueLabel);
        }

        static ChartEntry CreateEntry(float value, SKColor color, string label = null, string valueLabel = null)
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

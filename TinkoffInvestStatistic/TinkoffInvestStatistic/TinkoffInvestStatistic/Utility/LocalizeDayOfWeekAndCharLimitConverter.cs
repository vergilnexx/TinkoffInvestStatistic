using System;
using System.Globalization;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Utility
{
    /// <summary>
    /// Конвертер дней недели.
    /// </summary>
    public class LocalizeDayOfWeekAndCharLimitConverter : IValueConverter
    {
        /// <summary>
        /// Конвертация.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <param name="targetType">Ожидаемый тип.</param>
        /// <param name="parameter">Параметры.</param>
        /// <param name="culture">Культура.</param>
        /// <returns>Строка с названием дня недели.</returns>
        /// <exception cref="NotImplementedException">Исключение, бросаемое когда неизвестный день недели.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DayOfWeek dayOfWeek))
            {
                return string.Empty;
            }

            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Пн",
                DayOfWeek.Tuesday => "Вт",
                DayOfWeek.Wednesday => "Ср",
                DayOfWeek.Thursday => "Чт",
                DayOfWeek.Friday => "Пт",
                DayOfWeek.Saturday => "Сб",
                DayOfWeek.Sunday => "Вс",
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Конвертация обратно.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <param name="targetType">Ожидаемый тип.</param>
        /// <param name="parameter">Параметры.</param>
        /// <param name="culture">Культура.</param>
        /// <exception cref="NotImplementedException">Исключение, бросаемое всегда, так как не нужный функционал.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

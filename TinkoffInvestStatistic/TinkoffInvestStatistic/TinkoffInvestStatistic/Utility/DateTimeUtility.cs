using System;
using System.Collections.Generic;
using System.Linq;
using TinkoffInvestStatistic.Contracts.Enums;
using XCalendar.Core.Extensions;

namespace TinkoffInvestStatistic.Utility
{
    /// <summary>
    /// Помощь при работе с датами.
    /// </summary>
    public static class DateTimeUtility
    {
        /// <summary>
        /// Возвращает список дат с определенной периодечностью.
        /// </summary>
        /// <param name="periodType">Тип периодичности.</param>
        /// <param name="startDate">Дата начала отсчета.</param>
        /// <param name="endDate">Дата окончания отсчета.</param>
        /// <returns>Даты.</returns>
        public static IEnumerable<DateTime> GetPeriodDates(PeriodDatesType periodType, DateTime startDate, DateTime endDate)
        {
            if (periodType == PeriodDatesType.None)
            {
                return Array.Empty<DateTime>();
            }

            if (endDate <= startDate)
            {
                return Array.Empty<DateTime>();
            }

            return periodType switch
            {
                PeriodDatesType.Week => GetDatesWithDayOfWeekInYear(startDate, endDate),
                PeriodDatesType.Month => GetDatesWithMonthInYear(startDate, endDate),
                PeriodDatesType.Quarter => GetQuarterlyDatesFromGivenDate(startDate, endDate),
                PeriodDatesType.Year => Enumerable.Range(startDate.Year, endDate.Year - startDate.Year)
                                                                 .Select(year => GetDate(year, startDate.Month, startDate.Day)),
                PeriodDatesType.None => null,
                _ => null,
            };
        }

        private static IEnumerable<DateTime> GetDatesWithMonthInYear(DateTime date, DateTime endDate)
        {
            var startDate = date.FirstDayOfYear().AddDays(date.Day - 1);

            for (DateTime current = startDate; current < endDate; current = current.AddMonths(1))
            {
                yield return current;
            }
        }

        private static IEnumerable<DateTime> GetDatesWithDayOfWeekInYear(DateTime date, DateTime endDate)
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;
            int year = date.Year;

            DateTime startDate = GetDate(year, 1, 1);
            int daysUntilFirstDesiredDayOfWeek = ((int)dayOfWeek - (int)startDate.DayOfWeek + 7) % 7;
            DateTime firstDesiredDayOfWeek = startDate.AddDays(daysUntilFirstDesiredDayOfWeek);

            for (DateTime current = firstDesiredDayOfWeek; current < endDate; current = current.AddWeeks(1))
            {
                yield return current;
            }
        }

        private static IEnumerable<DateTime> GetQuarterlyDatesFromGivenDate(DateTime date, DateTime endDate)
        {
            for (DateTime current = date; current < endDate; current = current.AddMonths(3))
            {
                yield return current;
            }
        }
        private static DateTime GetDate(int year, int month, int day)
        {
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
        }
    }
}

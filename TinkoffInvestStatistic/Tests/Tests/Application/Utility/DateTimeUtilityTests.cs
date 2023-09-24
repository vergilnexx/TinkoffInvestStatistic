using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Utility;

namespace Tests.Application.Utility
{
    [TestClass]
    public class DateTimeUtilityTests
    {
        [TestMethod]
        public void GetPeriodDates_PeriodDatesNone_Empty()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2024, 1, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.None, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPeriodDates_EndDateLessStartDate_Empty()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2023, 1, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.Week, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPeriodDates_StartDateEqualEndDate_Empty()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.Week, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPeriodDates_PeriodWeekByMonth_FourDatesSameDayOfWeek()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 2, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.Week, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
            Assert.IsTrue(result.All(d => d.DayOfWeek == startDate.DayOfWeek));
            Assert.IsTrue(result.All(d => d.Month == startDate.Month));
            Assert.IsTrue(result.All(d => d.Year == startDate.Year));
        }

        [TestMethod]
        public void GetPeriodDates_PeriodMonthByMonth_DateSameStart()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 2, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.Month, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(startDate, result.FirstOrDefault());
            Assert.IsTrue(result.All(d => d.Day == startDate.Day));
            Assert.IsTrue(result.All(d => d.Year == startDate.Year));
        }

        [TestMethod]
        public void GetPeriodDates_PeriodMonthByYear_TwelveDateSameDayAndYear()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2024, 1, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.Month, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(12, result.Count());
            Assert.AreEqual(startDate, result.FirstOrDefault());
            Assert.IsTrue(result.All(d => d.Day == startDate.Day));
            Assert.IsTrue(result.All(d => d.Year == startDate.Year));
        }

        [TestMethod]
        public void GetPeriodDates_PeriodQuarterByYear_DateSameStart()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2024, 1, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.Quarter, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
            var dates = result.ToArray();
            Assert.AreEqual(new DateTime(2023, 1, 1), dates[0]);
            Assert.AreEqual(new DateTime(2023, 4, 1), dates[1]);
            Assert.AreEqual(new DateTime(2023, 7, 1), dates[2]);
            Assert.AreEqual(new DateTime(2023, 10, 1), dates[3]);
        }

        [TestMethod]
        public void GetPeriodDates_PeriodYear_DateSameStart()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2024, 1, 1);

            // Act
            var result = DateTimeUtility.GetPeriodDates(PeriodDatesType.Year, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(startDate, result.FirstOrDefault());
        }
    }
}

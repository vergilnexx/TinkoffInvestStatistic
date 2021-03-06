using Contracts;
using Contracts.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class PositionServiceTests
    {
        private PositionServiceForTest _service;

        [TestInitialize]
        public void Init()
        {
            _service = new PositionServiceForTest();
        }

        [TestMethod]
        public void GetSumByPositions_OnePositionUsd_CorrectCalculatedSumInRubles()
        {
            var positions = new[]
            {
                new Position("???????? ??????", PositionType.Etf) 
                { 
                    PositionCount = 20, 
                    AveragePositionPrice = new CurrencyMoney(Currency.Usd, 1m), 
                    ExpectedYield = new CurrencyMoney(Currency.Usd, -10m)
                },
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, Array.Empty<CurrencyMoney>(), currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(700m, sum); // (20 * 1 - 10) * 70 = 700
        }

        [TestMethod]
        public void GetSumByPositions_OnePositionEur_CorrectCalculatedSumInRubles()
        {
            var positions = new[]
            {
                new Position("???????? ??????", PositionType.Etf)
                {
                    PositionCount = 20,
                    AveragePositionPrice = new CurrencyMoney(Currency.Eur, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Eur, -10m)
                },
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, Array.Empty<CurrencyMoney>(), currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(800m, sum); // (20 * 1 - 10) * 80 = 800
        }

        [TestMethod]
        public void GetSumByPositions_OnePositionRub_CorrectCalculatedSumInRubles()
        {
            var positions = new[]
            {
                new Position("???????? ??????", PositionType.Etf)
                {
                    PositionCount = 20,
                    AveragePositionPrice = new CurrencyMoney(Currency.Rub, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Rub, -10m)
                },
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, Array.Empty<CurrencyMoney>(), currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(10m, sum); // 20 * 1 - 10 = 10
        }

        [TestMethod]
        public void GetSumByPositions_TwoPositionRub_CorrectCalculatedSumInRubles()
        {
            var positions = new[]
            {
                new Position("???????? ??????", PositionType.Etf)
                {
                    PositionCount = 20,
                    AveragePositionPrice = new CurrencyMoney(Currency.Rub, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Rub, -10m)
                },
                new Position("???????? ?????????", PositionType.Etf)
                {
                    PositionCount = 30,
                    AveragePositionPrice = new CurrencyMoney(Currency.Rub, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Rub, -15m)
                },
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, Array.Empty<CurrencyMoney>(), currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(25m, sum); // 20*1-10 + 30*1-15 = 25
        }

        [TestMethod]
        public void GetSumByPositions_TwoPositionUsd_CorrectCalculatedSumInRubles()
        {
            var positions = new[]
            {
                new Position("???????? ??????", PositionType.Etf)
                {
                    PositionCount = 20,
                    AveragePositionPrice = new CurrencyMoney(Currency.Usd, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Usd, -10m)
                },
                new Position("???????? ?????????", PositionType.Etf)
                {
                    PositionCount = 30,
                    AveragePositionPrice = new CurrencyMoney(Currency.Usd, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Usd, -15m)
                },
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, Array.Empty<CurrencyMoney>(), currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(1750m, sum); // (20*1-10 + 30*1-15) * 70 = 1750
        }

        [TestMethod]
        public void GetSumByPositions_TwoPositionEur_CorrectCalculatedSumInRubles()
        {
            var positions = new[]
            {
                new Position("???????? ??????", PositionType.Etf)
                {
                    PositionCount = 20,
                    AveragePositionPrice = new CurrencyMoney(Currency.Eur, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Eur, -10m)
                },
                new Position("???????? ?????????", PositionType.Etf)
                {
                    PositionCount = 30,
                    AveragePositionPrice = new CurrencyMoney(Currency.Eur, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Eur, -15m)
                },
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, Array.Empty<CurrencyMoney>(), currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(2000m, sum); // (20*1-10 + 30*1-15) * 80 = 2000
        }

        [TestMethod]
        public void GetSumByPositions_OneFiatPositionUsd_CorrectCalculatedSumInRubles()
        {
            var positions = Array.Empty<Position>();
            var fiatPositions = new[]
            {
                new CurrencyMoney(Currency.Usd, 5.5m),
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, fiatPositions, currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(385m, sum);
        }

        [TestMethod]
        public void GetSumByPositions_OneFiatPositionEur_CorrectCalculatedSumInRubles()
        {
            var positions = Array.Empty<Position>();
            var fiatPositions = new[]
            {
                new CurrencyMoney(Currency.Eur, 4m),
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, fiatPositions, currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(320m, sum);
        }

        [TestMethod]
        public void GetSumByPositions_OneFiatPositionRub_CorrectCalculatedSumInRubles()
        {
            var positions = Array.Empty<Position>();
            var fiatPositions = new[]
            {
                new CurrencyMoney(Currency.Rub, 10m),
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, fiatPositions, currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(10m, sum);
        }

        [TestMethod]
        public void GetSumByPositions_ThreeFiatPositionRubEurUsd_CorrectCalculatedSumInRubles()
        {
            var positions = Array.Empty<Position>();
            var fiatPositions = new[]
            {
                new CurrencyMoney(Currency.Usd, 10m),
                new CurrencyMoney(Currency.Eur, 20m),
                new CurrencyMoney(Currency.Rub, 30m),
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, fiatPositions, currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(2330m, sum);
        }

        [TestMethod]
        public void GetSumByPositions_OnePositionOneFiatPositionUsd_CorrectCalculatedSumInRubles()
        {
            var positions = new[]
            {
                new Position("???????? ??????", PositionType.Etf)
                {
                    PositionCount = 20,
                    AveragePositionPrice = new CurrencyMoney(Currency.Usd, 1m),
                    ExpectedYield = new CurrencyMoney(Currency.Usd, -10m)
                },
            };
            var fiatPositions = new[]
{
                new CurrencyMoney(Currency.Usd, 10m),
            };
            var currencies = new[]
            {
                new CurrencyMoney(Currency.Usd, 70m),
                new CurrencyMoney(Currency.Eur, 80m)
            };

            var sum = _service.GetSumByPositionsForTest(positions, fiatPositions, currencies);

            Assert.IsNotNull(sum);
            Assert.AreEqual(1400m, sum); // (20 * 1 - 10 + 10) * 70 = 1400
        }

    }
}
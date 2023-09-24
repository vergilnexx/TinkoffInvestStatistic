using Services;
using System.Collections.Generic;
using TinkoffInvestStatistic.Contracts;

namespace Tests.BusinessLogic.Services
{
    internal class PositionServiceForTest : PositionService
    {
        public decimal? GetSumByPositionsForTest(
            IReadOnlyCollection<Position> positions,
            IReadOnlyCollection<CurrencyMoney> fiatPositions,
            IReadOnlyCollection<CurrencyMoney> currencies)
        {
            return GetSumByPositions(positions, fiatPositions, currencies);
        }
    }
}

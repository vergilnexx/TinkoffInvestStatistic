using Contracts;
using Services;
using System.Collections.Generic;

namespace Tests
{
    internal class PositionServiceForTest : PositionService
    {
        public decimal? GetSumByPositionsForTest(IReadOnlyCollection<Position> positions, IReadOnlyCollection<CurrencyMoney> currencies)
        {
            return GetSumByPositions(positions, currencies);
        }
    }
}

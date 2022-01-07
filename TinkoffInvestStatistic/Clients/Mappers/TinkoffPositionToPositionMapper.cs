using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="Tinkoff.Trading.OpenApi.Models.Portfolio"/> в <see cref="Contracts.Account"/>
    /// </summary>
    public class TinkoffPositionToPositionMapper : IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<Contracts.Position>>
    {
        /// <inheritdoc/>
        public IReadOnlyCollection<Contracts.Position> Map(Portfolio portfolio)
        {
            return portfolio.Positions
                    .Select(p => Map(p))
                    .ToArray();
        }

        private Contracts.Position Map(Portfolio.Position position)
        {
            var result = new Contracts.Position(position.Figi, EnumMapper.MapType(position.InstrumentType));

            result.Name = position.Name;
            result.Ticker = position.Ticker;
            result.PositionCount = position.Balance;
            result.Blocked = position.Blocked;
            result.AveragePositionPrice = MapMoney(position.AveragePositionPrice);
            result.AveragePositionPriceNoNkd = MapMoney(position.AveragePositionPriceNoNkd);
            result.ExpectedYield = MapMoney(position.ExpectedYield);

            return result;
        }

        private Contracts.CurrencyMoney? MapMoney(MoneyAmount money)
        {
            if(money == null)
            {
                return null;
            }

            var result = new Contracts.CurrencyMoney(EnumMapper.MapCurrency(money.Currency), money.Value);

            return result;
        }
    }
}

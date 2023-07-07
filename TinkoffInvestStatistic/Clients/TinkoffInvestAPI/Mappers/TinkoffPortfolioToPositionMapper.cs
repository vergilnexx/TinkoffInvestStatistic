using System;
using System.Collections.Generic;
using System.Linq;
using TinkoffInvest.Contracts.Common;
using TinkoffInvest.Contracts.Portfolio;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="PortfolioReponse"/> в <see cref="TinkoffInvestStatistic.Contracts.Position"/>
    /// </summary>
    public class TinkoffPortfolioToPositionMapper : IMapper<PortfolioReponse, IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Position>>
    {
        /// <inheritdoc/>
        public IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Position> Map(PortfolioReponse portfolio)
        {
            return portfolio.Positions
                    .Select(p => Map(p))
                    .ToArray();
        }

        private TinkoffInvestStatistic.Contracts.Position Map(TinkoffInvest.Contracts.Portfolio.Position position)
        {
            var result = new TinkoffInvestStatistic.Contracts.Position(position.Figi, EnumMapper.MapInstruments(position.InstrumentType));

            result.Name = position.Name;
            result.Ticker = position.Ticker;
            result.PositionCount = position.Quantity.GetValue();
            result.AveragePositionPrice = MapMoney(position.AveragePositionPrice);
            result.ExpectedYield = MapMoney(position.ExpectedYield);

            return result;
        }

        private TinkoffInvestStatistic.Contracts.CurrencyMoney? MapMoney(CurrencyNumeric numeric)
        {
            if (numeric == null)
            {
                return null;
            }

            var result = new TinkoffInvestStatistic.Contracts.CurrencyMoney(
                                EnumMapper.MapCurrency(numeric.Currency), numeric.GetValue());
            return result;
        }
        private TinkoffInvestStatistic.Contracts.CurrencyMoney? MapMoney(Numeric numeric)
        {
            if (numeric == null)
            {
                return null;
            }

            var result = new TinkoffInvestStatistic.Contracts.CurrencyMoney(Currency.Rub, numeric.GetValue());
            return result;
        }
    }
}

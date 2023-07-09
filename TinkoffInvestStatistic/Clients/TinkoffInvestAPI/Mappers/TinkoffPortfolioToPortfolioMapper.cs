using System.Linq;
using TinkoffInvest.Contracts.Common;
using TinkoffInvest.Contracts.Enums;
using TinkoffInvest.Contracts.Portfolio;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="PortfolioReponse"/> в <see cref="Portfolio"/>
    /// </summary>
    public class TinkoffPortfolioToPortfolioMapper : IMapper<PortfolioReponse, Portfolio>
    {
        /// <inheritdoc/>
        public Portfolio Map(PortfolioReponse portfolio)
        {
            var positions = portfolio.Positions
                                .Select(p => Map(p))
                                .ToArray();
            var result = new Portfolio(portfolio.AccountId!, MapMoney(portfolio.TotalAmount), positions)
            {
                TotalAmountStocks = MapMoney(portfolio.TotalAmountShares),
                TotalAmountBonds = MapMoney(portfolio.TotalAmountBonds),
                TotalAmountEtf = MapMoney(portfolio.TotalAmountEtf),
                TotalAmountCurrencies = MapMoney(portfolio.TotalAmountCurrencies)
            };
            return result;
        }

        private TinkoffInvestStatistic.Contracts.Position Map(TinkoffInvest.Contracts.Portfolio.Position position)
        {
            var result = new TinkoffInvestStatistic.Contracts.Position(position.Figi, EnumMapper.MapInstruments(position.InstrumentType));

            result.Name = position.Figi;
            result.Ticker = position.Figi;
            result.Currency = EnumMapper.MapCurrency(position.AveragePositionPrice?.Currency ?? CurrencyType.Rub);

            var quantity = position.Quantity.GetValue();
            result.PositionCount = quantity;
            result.Sum = position.CurrencyCurrentPrice.GetValue() * quantity;
            result.SumInCurrency = position.CurrencyCurrentPrice.GetValue() * quantity;
            result.AveragePositionPrice = MapMoney(position.AveragePositionPrice);
            result.ExpectedYield = MapMoney(position.ExpectedYield);
            result.CurrentPrice = MapMoney(position.CurrencyCurrentPrice);

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

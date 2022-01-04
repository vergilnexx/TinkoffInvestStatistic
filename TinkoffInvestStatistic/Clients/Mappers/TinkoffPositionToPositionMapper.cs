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
            var result = new Contracts.Position();

            result.Figi = position.Figi;
            result.Name = position.Name;
            result.Ticker = position.Ticker;
            result.PositionCount = position.Balance;
            result.Blocked = position.Blocked;
            result.AveragePositionPrice = MapMoney(position.AveragePositionPrice);
            result.AveragePositionPriceNoNkd = MapMoney(position.AveragePositionPriceNoNkd);
            result.ExpectedYield = MapMoney(position.ExpectedYield);
            result.Type = MapType(position.InstrumentType);

            return result;
        }

        private Contracts.CurrencyMoney? MapMoney(MoneyAmount money)
        {
            if(money == null)
            {
                return null;
            }

            var result = new Contracts.CurrencyMoney(MapCurrency(money.Currency), money.Value);

            return result;
        }

        private Contracts.Enums.Currency MapCurrency(Tinkoff.Trading.OpenApi.Models.Currency currency)
        {
            switch (currency)
            {
                case Tinkoff.Trading.OpenApi.Models.Currency.Rub:
                    return Contracts.Enums.Currency.Rub;
                case Tinkoff.Trading.OpenApi.Models.Currency.Usd:
                    return Contracts.Enums.Currency.Usd;
                case Tinkoff.Trading.OpenApi.Models.Currency.Eur:
                    return Contracts.Enums.Currency.Eur;
                case Tinkoff.Trading.OpenApi.Models.Currency.Gbp:
                    return Contracts.Enums.Currency.Gbp;
                case Tinkoff.Trading.OpenApi.Models.Currency.Hkd:
                    return Contracts.Enums.Currency.Hkd;
                case Tinkoff.Trading.OpenApi.Models.Currency.Chf:
                    return Contracts.Enums.Currency.Chf;
                case Tinkoff.Trading.OpenApi.Models.Currency.Jpy:
                    return Contracts.Enums.Currency.Jpy;
                case Tinkoff.Trading.OpenApi.Models.Currency.Cny:
                    return Contracts.Enums.Currency.Cny;
                case Tinkoff.Trading.OpenApi.Models.Currency.Try:
                    return Contracts.Enums.Currency.Try;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currency));
            }
        }

        private PositionType MapType(InstrumentType type)
        {
            switch (type)
            {
                case InstrumentType.Stock:
                    return PositionType.Stock;
                case InstrumentType.Currency:
                    return PositionType.Currency;
                case InstrumentType.Bond:
                    return PositionType.Bond;
                case InstrumentType.Etf:
                    return PositionType.Etf;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}

using Contracts.Enums;
using System;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер перечислений.
    /// </summary>
    internal static class EnumMapper
    {
        public static Contracts.Enums.Currency MapCurrency(Tinkoff.Trading.OpenApi.Models.Currency currency)
        {
            return currency switch
            {
                Tinkoff.Trading.OpenApi.Models.Currency.Rub => Contracts.Enums.Currency.Rub,
                Tinkoff.Trading.OpenApi.Models.Currency.Usd => Contracts.Enums.Currency.Usd,
                Tinkoff.Trading.OpenApi.Models.Currency.Eur => Contracts.Enums.Currency.Eur,
                Tinkoff.Trading.OpenApi.Models.Currency.Gbp => Contracts.Enums.Currency.Gbp,
                Tinkoff.Trading.OpenApi.Models.Currency.Hkd => Contracts.Enums.Currency.Hkd,
                Tinkoff.Trading.OpenApi.Models.Currency.Chf => Contracts.Enums.Currency.Chf,
                Tinkoff.Trading.OpenApi.Models.Currency.Jpy => Contracts.Enums.Currency.Jpy,
                Tinkoff.Trading.OpenApi.Models.Currency.Cny => Contracts.Enums.Currency.Cny,
                Tinkoff.Trading.OpenApi.Models.Currency.Try => Contracts.Enums.Currency.Try,
                _ => throw new ArgumentOutOfRangeException(nameof(currency)),
            };
        }

        public static PositionType MapType(InstrumentType type)
        {
            return type switch
            {
                InstrumentType.Stock => PositionType.Stock,
                InstrumentType.Currency => PositionType.Currency,
                InstrumentType.Bond => PositionType.Bond,
                InstrumentType.Etf => PositionType.Etf,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }
    }
}

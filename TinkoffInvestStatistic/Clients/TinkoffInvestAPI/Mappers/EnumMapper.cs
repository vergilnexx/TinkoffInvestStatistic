using System;
using TinkoffInvest.Contracts.Enums;
using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер перечислений.
    /// </summary>
    internal static class EnumMapper
    {
        /// <summary>
        /// Маппинг валют.
        /// </summary>
        /// <param name="currency">Входная валюта.</param>
        /// <returns>Валюта из контрактов.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Исключение при неизвестной валюте.</exception>
        public static TinkoffInvestStatistic.Contracts.Enums.Currency MapCurrency(CurrencyType currency)
        {
            return currency switch
            {
                CurrencyType.Rub => TinkoffInvestStatistic.Contracts.Enums.Currency.Rub,
                CurrencyType.Usd => TinkoffInvestStatistic.Contracts.Enums.Currency.Usd,
                CurrencyType.Eur => TinkoffInvestStatistic.Contracts.Enums.Currency.Eur,
                CurrencyType.Hkd => TinkoffInvestStatistic.Contracts.Enums.Currency.Hkd,
                CurrencyType.Cny => TinkoffInvestStatistic.Contracts.Enums.Currency.Cny,
                _ => throw new ArgumentOutOfRangeException(nameof(currency)),
            };
        }

        /// <summary>
        /// Маппинг инструментов.
        /// </summary>
        /// <param name="type">Входной тип.</param>
        /// <returns>Тип из контрактов.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Исключение при неизвестном типе.</exception>
        public static PositionType MapInstruments(InstrumentType type)
        {
            return type switch
            {
                InstrumentType.Share => PositionType.Stock,
                InstrumentType.Currency => PositionType.Currency,
                InstrumentType.Bond => PositionType.Bond,
                InstrumentType.Etf => PositionType.Etf,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }
    }
}

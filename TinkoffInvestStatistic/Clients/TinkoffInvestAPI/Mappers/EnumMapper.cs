using System;
using TinkoffInvest.Contracts.Enums;
using TinkoffInvestStatistic.Contracts.Enums;
using AccountType = TinkoffInvestStatistic.Contracts.Enums.AccountType;
using TinkoffContracts = TinkoffInvest.Contracts;

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

        /// <summary>
        /// Маппинг типов счетов.
        /// </summary>
        /// <param name="brokerAccountType">Входной тип.</param>
        /// <returns>Тип из контрактов.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Исключение при неизвестном типе.</exception>
        public static AccountType MapAccountType(TinkoffContracts.Enums.AccountType brokerAccountType)
        {
            return brokerAccountType switch
            {
                TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF => AccountType.BrokerAccount,
                TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF_IIS => AccountType.Iis,
                TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_INVEST_BOX => AccountType.InvestBox,
                _ => throw new ArgumentOutOfRangeException(nameof(brokerAccountType)),
            };
        }
    }
}

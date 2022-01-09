using Contracts;
using Contracts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="Tinkoff.Trading.OpenApi.Models.Portfolio"/> в <see cref="Contracts.CurrencyMoney"/>
    /// </summary>
    public class TinkoffPortfolioToCurrencyMoneyMapper : IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<Contracts.CurrencyMoney>>
    {
        /// <inheritdoc/>
        public IReadOnlyCollection<CurrencyMoney> Map(Portfolio list)
        {
            var currencies = Enum.GetValues(typeof(Contracts.Enums.Currency)).Cast<Contracts.Enums.Currency>();
            var result = list.Positions
                                .Where<Portfolio.Position>(p => p.InstrumentType == InstrumentType.Currency)
                                .Select(i => new CurrencyMoney(
                                    currencies.FirstOrDefault(c => IsThisCurrency(i, c)), 
                                    i.AveragePositionPrice.Value + i.ExpectedYield.Value / i.Balance))
                                .ToArray();
            return result;
        }

        private static bool IsThisCurrency(Portfolio.Position i, Contracts.Enums.Currency c)
        {
            var figi = GetFigi(c);
            return figi == i.Figi;
        }

        private static string GetFigi(Contracts.Enums.Currency currency)
        {
            var enumType = currency.GetType();
            var name = Enum.GetName(enumType, currency);
            var value = enumType.GetField(name).GetCustomAttributes(inherit: false).OfType<FigiAttribute>().FirstOrDefault()?.Value;
            return value;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="Tinkoff.Trading.OpenApi.Models.PortfolioCurrencies"/> в <see cref="Contracts.CurrencyMoney"/>
    /// </summary>
    public class TinkoffPortfolioCurrenciesToPositionMapper : IMapper<Tinkoff.Trading.OpenApi.Models.PortfolioCurrencies, IReadOnlyCollection<Contracts.CurrencyMoney>>
    {
        /// <inheritdoc/>
        public IReadOnlyCollection<Contracts.CurrencyMoney> Map(PortfolioCurrencies portfolio)
        {
            return portfolio.Currencies
                    .Select(p => Map(p))
                    .ToArray();
        }

        private Contracts.CurrencyMoney Map(PortfolioCurrencies.PortfolioCurrency position)
        {
            var result = new Contracts.CurrencyMoney(EnumMapper.MapCurrency(position.Currency), position.Balance);

            return result;
        }
    }
}

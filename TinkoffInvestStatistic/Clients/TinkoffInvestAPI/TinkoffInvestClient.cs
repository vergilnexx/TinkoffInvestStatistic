using Contracts;
using Infrastructure.Clients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffInvest;
using TinkoffInvest.Mappers;
using Xamarin.Forms;

namespace Clients.TinkoffInvest
{
    /// <summary>
    /// Клиент для работы с API "Тинькофф Инвестиции".
    /// </summary>
    public class TinkoffInvestClient : IBankBrokerApiClient
    {
        /// <summary>
        /// Токен.
        /// </summary>
        private static readonly string Token = Secrets.TINKOFF_INVEST_TOKEN;

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Contracts.Account>> GetAccountsAsync()
        {
            using var connection = ConnectionFactory.GetConnection(Token);
            var context = connection.Context;

            var accounts = await context.AccountsAsync().ConfigureAwait(continueOnCapturedContext: false);
            var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.Account, Contracts.Account>>();
            var result = accounts.Select(a => mapper.Map(a)).ToArray();

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Position>> GetPositionsAsync(string accountId)
        {
            using var connection = ConnectionFactory.GetConnection(Token);
            var context = connection.Context;

            var portfolio = await context.PortfolioAsync(accountId).ConfigureAwait(continueOnCapturedContext: false);
            var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<Contracts.Position>>>();
            var result = mapper.Map(portfolio);

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<CurrencyMoney>> GetCurrenciesAsync()
        {
            using var connection = ConnectionFactory.GetConnection(Token);
            var context = connection.Context;

            var currencies = await context.PortfolioAsync().ConfigureAwait(continueOnCapturedContext: false);
            var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<Contracts.CurrencyMoney>>>();
            var result = mapper.Map(currencies);

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<CurrencyMoney>> GetFiatPositionsAsync(string accountId)
        {
            using var connection = ConnectionFactory.GetConnection(Token);
            var context = connection.Context;

            var portfolio = await context.PortfolioCurrenciesAsync(accountId).ConfigureAwait(continueOnCapturedContext: false);
            var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.PortfolioCurrencies, IReadOnlyCollection<Contracts.CurrencyMoney>>>();
            var result = mapper.Map(portfolio);

            return result;
        }
    }
}

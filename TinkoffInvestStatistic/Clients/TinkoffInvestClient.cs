using Contracts;
using Infrastructure.Clients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Network;
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
        /// Токен песочницы.
        /// </summary>
        const string TINKOFF_INVEST_SANDBOX_TOKEN = "";

        /// <summary>
        /// Токен.
        /// </summary>
        const string TINKOFF_INVEST_TOKEN = "";

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Contracts.Account>> GetAccountsAsync()
        {
            using var connection = ConnectionFactory.GetConnection(TINKOFF_INVEST_TOKEN);
            var context = connection.Context;

            var accounts = await context.AccountsAsync().ConfigureAwait(continueOnCapturedContext: false);
            var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.Account, Contracts.Account>>();
            var result = accounts.Select(a => mapper.Map(a)).ToArray();

            return result;
        }
    }
}

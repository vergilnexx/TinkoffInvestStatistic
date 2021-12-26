using Contracts;
using Infrastructure.Clients;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class AccountService : IAccountService
    {
    /// <inheritdoc/>
        public Task<IReadOnlyCollection<Account>> GetAccountsAsync()
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            return bankBrokerClient.GetAccountsAsync();
        }
    }
}

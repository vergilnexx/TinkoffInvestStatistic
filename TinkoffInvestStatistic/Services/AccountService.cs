using Contracts;
using Infrastructure.Clients;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class AccountService : IAccountService
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Account>> GetAccountsAsync()
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var externalAccounts = await bankBrokerClient.GetAccountsAsync();

            var positionService = DependencyService.Resolve<IPositionService>();
            foreach (var account in externalAccounts)
            {
                account.Sum = await positionService.GetPositionsSumAsync(account.ID);
            }

            var accounts = await DataStorageService.Instance.MergeAccountData(externalAccounts);

            return accounts.ToArray();
        }
    }
}

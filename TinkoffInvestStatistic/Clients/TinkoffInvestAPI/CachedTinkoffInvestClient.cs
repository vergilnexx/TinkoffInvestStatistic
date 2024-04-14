using Clients.TinkoffInvest;
using Infrastructure.Clients;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;
using Xamarin.Forms;

namespace TinkoffInvest
{
    /// <summary>
    /// Клиент для работы с API "Тинькофф Инвестиции" с кэшированием данных.
    /// </summary>
    public class CachedTinkoffInvestClient : IBankBrokerApiClient
    {
        private readonly IBankBrokerApiClient _bankBrokerClient;
        private readonly ICacheService _cacheService;

        public CachedTinkoffInvestClient() 
        {
            _bankBrokerClient = DependencyService.Resolve<TinkoffInvestClient>();
            _cacheService = DependencyService.Resolve<ICacheService>();
        }

        /// <inheritdoc/>
        public async Task<Position> FindPositionByFigiAsync(string figi, PositionType positionType)
        {
            return await _cacheService.GetOrCreateAsync(
                            "position_" + figi,
                            cacheExpirationInMinutes: 5,
                            () => _bankBrokerClient.FindPositionByFigiAsync(figi, positionType));
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Account>> GetAccountsAsync()
        {
            return await _cacheService.GetOrCreateAsync(
                            "accounts",
                            cacheExpirationInMinutes: 5,
                            () => _bankBrokerClient.GetAccountsAsync());
        }

        /// <inheritdoc/>
        public async Task<Portfolio> GetAccountsFullDataAsync(string accountNumber)
        {
            return await _cacheService.GetOrCreateAsync(
                            "account_" + accountNumber,
                            cacheExpirationInMinutes: 10,
                            () => _bankBrokerClient.GetAccountsFullDataAsync(accountNumber));
        }
    }
}

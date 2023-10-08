using Clients.TinkoffInvest;
using Infrastructure.Clients;
using Microsoft.Extensions.Caching.Memory;
using System;
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
        private readonly MemoryCache _cache;

        public CachedTinkoffInvestClient() 
        {
            _bankBrokerClient = DependencyService.Resolve<TinkoffInvestClient>();
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        /// <inheritdoc/>
        public async Task<Position> FindPositionByFigiAsync(string figi, PositionType positionType)
        {
            return await _cache.GetOrCreate(
                            "position_" + figi,
                            cacheEntry =>
                            {
                                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(5);
                                return _bankBrokerClient.FindPositionByFigiAsync(figi, positionType);
                            });
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Account>> GetAccountsAsync()
        {
            return await _cache.GetOrCreate(
                            "accounts",
                            cacheEntry =>
                            {
                                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(5);
                                return _bankBrokerClient.GetAccountsAsync();
                            });
        }

        /// <inheritdoc/>
        public async Task<Portfolio> GetAccountsFullDataAsync(string accountNumber)
        {
            return await _cache.GetOrCreate(
                            "account_" + accountNumber,
                            cacheEntry =>
                            {
                                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);
                                return _bankBrokerClient.GetAccountsFullDataAsync(accountNumber);
                            });
        }
    }
}

using Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Services
{
    /// <inheritdoc/>
    public class CacheService : ICacheService
    {
        private MemoryCache _cache;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CacheService()
        {
            _cache = CreateNewCache();
        }

        /// <inheritdoc/>
        public async Task<T> GetOrCreateAsync<T>(string key, int cacheExpirationInMinutes, Func<Task<T>> action) where T : class
        {
            return await _cache.GetOrCreate(key,
                            cacheEntry =>
                            {
                                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(cacheExpirationInMinutes);
                                return action();
                            });
        }

        /// <inheritdoc/>
        public void ResetCache()
        {
            _cache.Dispose();
            _cache = CreateNewCache();
        }

        private static MemoryCache CreateNewCache()
        {
            return new MemoryCache(new MemoryCacheOptions());
        }
    }
}

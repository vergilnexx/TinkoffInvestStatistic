using System.Threading.Tasks;
using System;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис для работы с кэшом.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Берет результат из кэша или, в случае отсутствия, выполняет действие и кладет результат в кэш.
        /// </summary>
        /// <typeparam name="T">Тип результата действия.</typeparam>
        /// <param name="key">Ключ кэша.</param>
        /// <param name="cacheExpirationInMinutes">Время действия кэша в минутах.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат действия.</returns>
        Task<T> GetOrCreateAsync<T>(string key, int cacheExpirationInMinutes, Func<Task<T>> action) where T : class;

        /// <summary>
        /// Сбрасывает кэш.
        /// </summary>
        void ResetCache();
    }
}

using System.Threading.Tasks;

namespace TinkoffInvestStatistic.Service
{
    /// <summary>
    /// Helper для работы с аутентификацией.
    /// </summary>
    internal interface IAuthenticateService
    {
        /// <summary>
        /// Возвращает признак, что аутентификация пройдена.
        /// </summary>
        /// <param name="queryName">Название запроса.</param>
        /// <returns>Признак, что аутентификация пройдена.</returns>
        Task<bool> AuthenticateAsync(string queryName);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис по работе с секторами.
    /// </summary>
    public interface ISectorService
    {
        /// <summary>
        /// Возвращает список секторов.
        /// </summary>
        /// <returns>Список секторов.</returns>
        Task<IReadOnlyCollection<Sector>> GetSectorsAsync();

        /// <summary>
        /// Возвращает информацию по сектору.
        /// </summary>
        /// <param name="sectorId">Идентификатор сектора.</param>
        /// <returns>Сектор.</returns>
        Task<Sector> GetSectorAsync(int sectorId);

        /// <summary>
        /// Добавление сектора.
        /// </summary>
        /// <param name="sector">Сектор.</param>
        Task AddSectorAsync(Sector sector);

        /// <summary>
        /// Обновление сектора.
        /// </summary>
        /// <param name="sector">Сектор.</param>
        Task UpdateSectorAsync(Sector sector);
    }
}

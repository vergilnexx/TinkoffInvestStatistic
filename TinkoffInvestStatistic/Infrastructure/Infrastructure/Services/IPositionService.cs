using Contracts;
using Contracts.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис позиций по счет
    /// </summary>
    public interface IPositionService
    {
        /// <summary>
        /// Возвращает список позиций по счету.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <returns>Группированный список позиций только для чтения.</returns>
        public Task<Dictionary<PositionType, Position[]>> GetGroupedPositionsAsync(string accountId);
    }
}

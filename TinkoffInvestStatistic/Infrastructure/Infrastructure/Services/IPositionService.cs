using Contracts;
using Contracts.Enums;
using Domain;
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
        /// Возвращает список позиций определенного типа по счету.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <param name="positionType">Тип позиции.</param>
        /// <returns>Группированный список позиций только для чтения.</returns>
        public Task<IReadOnlyCollection<Position>> GetGroupedPositionsAsync(string accountId, PositionType positionType);

        /// <summary>
        /// Возвращает список позиций по счету.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <param name="positionType">Тип инструмента.</param>
        /// <returns>Группированный список позиций только для чтения.</returns>
        public Task<decimal> GetPositionsSumAsync(string accountId, PositionType positionType);

        /// <summary>
        /// Расчитывает сумма по всем позициям на счете.
        /// </summary>
        /// <param name="accountId">Номер счета.</param>
        /// <returns>Сумма.</returns>
        public Task<decimal> GetPositionsSumAsync(string accountId);
        
        /// <summary>
        /// Сохранение данных позиций по счету.
        /// </summary>
        /// <param name="accountId">Номер счета.</param>
        /// <param name="data">Данные по позициям</param>
        public Task SavePlanPercents(string accountId, PositionData[] data);
    }
}

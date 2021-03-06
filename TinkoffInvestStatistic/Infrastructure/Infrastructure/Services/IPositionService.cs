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
        public Task<IReadOnlyCollection<Position>> GetPositionsByTypeAsync(string accountId, PositionType positionType);

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
        /// <param name="positionType">Тип позиций.</param>
        /// <param name="data">Данные по позициям</param>
        public Task SavePlanPercents(string accountId, PositionType positionType, PositionData[] data);
        
        /// <summary>
        /// Возвращает позиции с определенным типом с фильтрацией по тикеру или названию.
        /// </summary>
        /// <param name="positionType">Тип позиции</param>
        /// <param name="name">Наименование или тикер</param>
        /// <returns>Список позиций.</returns>
        public Task<IReadOnlyCollection<Position>> GetPositionByTickerAsync(PositionType positionType, string name);

        /// <summary>
        /// Добавляет планируемую для покупки позицию.
        /// </summary>
        /// <param name="accountId">Номер счета.</param>
        /// <param name="type">Тип позиции.</param>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="ticker">тикер.</param>
        public Task AddPlannedPositionAsync(string accountId, PositionType type, string figi, string name, string ticker);
    }
}

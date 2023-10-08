using TinkoffInvestStatistic.Contracts.Enums;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

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
        /// <param name="accountNumber">Идентификатор счета.</param>
        /// <param name="positionType">Тип позиции.</param>
        /// <returns>Группированный список позиций только для чтения.</returns>
        public Task<IReadOnlyCollection<Position>> GetPositionsByTypeAsync(string accountNumber, PositionType positionType);

        /// <summary>
        /// Сохранение данных позиций по счету.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionType">Тип позиций.</param>
        /// <param name="data">Данные по позициям</param>
        public Task SavePlanPercents(string accountNumber, PositionType positionType, PositionData[] data);

        /// <summary>
        /// Добавляет планируемую для покупки позицию.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="type">Тип позиции.</param>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="ticker">тикер.</param>
        public Task AddPlannedPositionAsync(string accountNumber, PositionType type, string figi, string name, string ticker);
    }
}

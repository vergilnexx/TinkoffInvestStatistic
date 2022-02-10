using Contracts.Enums;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Интерфейс работы с данными.
    /// </summary>
    public interface IDataStorageAccessService
    {
        /// <summary>
        /// Возвращает данные о счетах.
        /// </summary>
        /// <returns>Данные о счетах.</returns>
        Task<IReadOnlyCollection<AccountData>> GetAccountDataAsync();

        /// <summary>
        /// Возвращает инструменты по счету из хранилища.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionTypes">Типы позиций.</param>
        /// <returns>Данные по инструментам.</returns>
        Task<IReadOnlyCollection<PositionTypeData>> GetPositionTypesAsync(string accountNumber, PositionType[] positionTypes);

        /// <summary>
        /// Возвращает информацию по позициям.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionType">Тип позиции.</param>
        /// <param name="data">Данные о позициям.</param>
        Task<IReadOnlyCollection<PositionData>> GetPositionsAsync(string accountNumber, PositionType positionType);

        /// <summary>
        /// Возвращает данные по валютам по счету из хранилища.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <returns>Данные по валютам.</returns>
        Task<IReadOnlyCollection<CurrencyData>> GetCurrenciesDataAsync(string accountNumber);

        /// <summary>
        /// Возвращает сектора.
        /// </summary>
        /// <returns>Сектора.</returns>
        Task<IReadOnlyCollection<SectorData>> GetSectorsAsync();

        /// <summary>
        /// Возвращает информацию по сектору.
        /// </summary>
        /// <param name="sectorId">Идентификатор сектора.</param>
        /// <returns>Сектор.</returns>
        Task<SectorData> GetSectorAsync(int sectorId);

        /// <summary>
        /// Сохранение данных о счетах.
        /// </summary>
        Task SaveAccountDataAsync(AccountData[] data);
        
        /// <summary>
        /// Сохранение данных по типам позиций
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные</param>
        Task SavePositionTypesDataAsync(string accountNumber, IReadOnlyCollection<PositionTypeData> data);
        
        /// <summary>
        /// Сохраняет данные по валютам.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные.</param>
        Task SaveCurrenciesDataAsync(string accountNumber, IReadOnlyCollection<CurrencyData> data);
        
        /// <summary>
        /// Сохранение данных по позициям.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные.</param>
        Task SavePositionsDataAsync(string accountNumber, PositionData[] data);

        /// <summary>
        /// Добавление сектора.
        /// </summary>
        /// <param name="sector">Сектор.</param>
        Task AddSectorAsync(SectorData sectorData);

        /// <summary>
        /// Обновление сектора.
        /// </summary>
        /// <param name="sector">Сектор.</param>
        Task UpdateSectorAsync(SectorData sectorData);
    }
}

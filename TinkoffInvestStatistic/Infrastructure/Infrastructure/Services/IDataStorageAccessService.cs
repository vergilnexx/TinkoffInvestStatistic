using TinkoffInvestStatistic.Contracts.Enums;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using System.Threading;

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
        /// Возвращает данные о планируемых для покупки позиций.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <param name="type">Тип позиции.</param>
        /// <returns>Данные о планируемых для покупки позиций.</returns>
        Task<IReadOnlyCollection<PlannedPositionData>> GetPlannedPositionsAsync(string accountId, PositionType type);

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

        /// <summary>
        /// Добавление планируемой для покупки позиции
        /// </summary>
        /// <param name="accountId">Номер счета.</param>
        /// <param name="type">Тип позиции</param>
        /// <param name="figi">Финансовый идентификатор</param>
        /// <param name="name">Наименование</param>
        /// <param name="ticker">Тикер</param>
        Task AddPlannedPositionAsync(string accountId, PositionType type, string figi, string name, string ticker);

        /// <summary>
        /// Возвращает настройки.
        /// </summary>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Настройки.</returns>
        Task<IReadOnlyCollection<OptionData>> GetOptionsAsync(CancellationToken cancellation);

        /// <summary>
        /// Обновляет данные настройки.
        /// </summary>
        /// <param name="type">Тип настройки.</param>
        /// <param name="value">Значение.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task UpdateOptionAsync(OptionType type, string value, CancellationToken cancellation);

        /// <summary>
        /// Возвращает значение настройки.
        /// </summary>
        /// <param name="optionType">Тип настройки.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список настроек.</returns>
        Task<string?> GetOptionAsync(OptionType optionType, CancellationToken cancellation);

        /// <summary>
        /// Возвращает список зачислений по брокерам.
        /// </summary>
        /// <returns>Список зачислений по брокерам.</returns>
        Task<IReadOnlyCollection<TransferBrokerData>> GetTransfersAsync(CancellationToken cancellation);

        /// <summary>
        /// Возвращает данные зачислений по брокеру.
        /// </summary>
        /// <param name="brokerName">Наименование брокера.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Зачисления.</returns>
        Task<TransferBrokerData> GetTransferAsync(string brokerName, CancellationToken cancellation);

        /// <summary>
        /// Сохраняет данные зачислений по брокеру.
        /// </summary>
        /// <param name="brokerName">Наименование брокера.</param>
        /// <param name="sum">Сумма.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task SaveTransferAsync(string brokerName, decimal sum, CancellationToken cancellation);
    }
}

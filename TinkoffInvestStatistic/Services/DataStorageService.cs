﻿using TinkoffInvestStatistic.Contracts.Enums;
using Domain;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using Xamarin.Forms;
using System.Threading;
using TinkoffInvestStatistic.Contracts.Export;
using System.Collections.ObjectModel;

namespace Services
{
    /// <summary>
    /// Сервис хранения данных
    /// </summary>
    public sealed class DataStorageService
    {
        /// <summary>
        /// Экземпляр.
        /// </summary>
        internal static DataStorageService Instance { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public DataStorageService()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of DataStorageService is allowed!");
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Объединение данных полученных из внешнего источника с локальными данными.
        /// </summary>
        /// <param name="externalAccounts">Внешние данные по счетам.</param>
        /// <returns>Данные по счетам.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal async Task<Account[]> MergeAccountData(IReadOnlyCollection<Account> externalAccounts)
        {
            if (externalAccounts == null)
            {
                throw new ArgumentNullException(nameof(externalAccounts), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var accounts = await dataAccessService.GetAccountDataAsync();
            var result = (accounts ?? Array.Empty<AccountData>()).ToList();
            foreach (var externalAccount in externalAccounts)
            {
                var account = result.FirstOrDefault(x => x.Number == externalAccount.ID);
                if(account == null)
                {
                    account = new AccountData(externalAccount.ID);
                    result.Add(account);
                }
            }

            await dataAccessService.SaveAccountDataAsync(result.ToArray());
            return externalAccounts.ToArray();
        }

        /// <summary>
        /// Возвращает планируемые для покупки позиции.
        /// </summary>
        /// <param name="accountId">Номер счета.</param>
        /// <param name="positionType">Тип позиций.</param>
        /// <returns>Планируемые для покупки позиции.</returns>
        internal async Task<IReadOnlyCollection<Position>> GetPlannedPositionsAsync(string accountId, PositionType positionType)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var positions = await dataAccessService.GetPlannedPositionsAsync(accountId, positionType);
            return positions.Select(p => new Position(p.Figi, positionType, p.Name, default, Currency.Rub) { Ticker = p.Ticker }).ToArray();
        }

        /// <summary>
        /// Возвращает заполненные данные по инструментам.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionTypes">Типы инструментов.</param>
        /// <returns>Заполненные данные по инструментам</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        internal async Task<Instrument[]> MergePositionTypesData(string accountNumber, PositionType[] positionTypes)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (positionTypes == null)
            {
                throw new ArgumentNullException(nameof(positionTypes), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var instrumentEntities = await dataAccessService.GetPositionTypesAsync(accountNumber, positionTypes);

            var result = new List<Instrument>();
            foreach (var type in positionTypes)
            {
                var instrumentEntity = instrumentEntities.FirstOrDefault(i => i.Type == type);
                if(instrumentEntity == null)
                {
                    instrumentEntity = new PositionTypeData(type);
                }

                result.Add(new Instrument(type, instrumentEntity?.PlanPercent ?? 0));
            }
            
            return result.ToArray();
        }

        /// <summary>
        /// Возвращает заполненные данные по инструментам.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="currencies">Данные по валюте.</param>
        /// <returns>Заполненные данные по инструментам</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        internal async Task<AccountCurrencyData[]> MergeCurrenciesData(string accountNumber, AccountCurrencyData[] currencies)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (currencies == null)
            {
                throw new ArgumentNullException(nameof(currencies), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var currenciesEntities = await dataAccessService.GetCurrenciesDataAsync(accountNumber);

            var result = new List<AccountCurrencyData>();
            foreach (var currency in currencies)
            {
                var currencyEntity = currenciesEntities.FirstOrDefault(i => i.Currency == currency.Currency);
                if (currencyEntity != null)
                {
                    currencyEntity.PlanPercent = currencyEntity?.PlanPercent ?? 0;
                }

                result.Add(new AccountCurrencyData(currency.Currency, currencyEntity?.PlanPercent ?? 0, currency.Sum));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Добавление планируемой для покупки позиции
        /// </summary>
        /// <param name="accountId">Номер счета.</param>
        /// <param name="type">Тип позиции</param>
        /// <param name="figi">Финансовый идентификатор</param>
        /// <param name="name">Наименование</param>
        internal async Task AddPlannedPositionAsync(string accountId, PositionType type, string figi, string name, string ticker)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.AddPlannedPositionAsync(accountId, type, figi, name, ticker);
        }

        /// <summary>
        /// Возвращает заполненные данные по позициям счета.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positions">Позиции.</param>
        /// <returns>Список заполненных позиций.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        internal async Task<IEnumerable<Position>> MergePositionData(string accountNumber, PositionType positionType, IReadOnlyCollection<Position> positions)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (positions == null || positions.Count() == 0)
            {
                throw new ArgumentNullException(nameof(positions), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var positionEntities = await dataAccessService.GetPositionsAsync(accountNumber, positionType);

            var result = new List<PositionData>();
            foreach (var position in positions)
            {
                var positionData = positionEntities.FirstOrDefault(i => i.Figi == position.Figi);
                if (positionData == null)
                {
                    positionData = new PositionData(accountNumber, position.Figi, position.Type);
                }
                else
                {
                    position.PlanPercent = positionData.PlanPercent;
                }
                result.Add(positionData);
            }

            return positions.ToArray();
        }

        /// <summary>
        /// Возвращает сектора.
        /// </summary>
        /// <returns>Сектора.</returns>
        internal async Task<IReadOnlyCollection<Sector>> GetSectorsAsync()
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var data = await dataAccessService.GetSectorsAsync();
            return data.Select(s => new Sector(s.Id, s.Name)).ToArray();
        }

        /// <summary>
        /// Возвращает информацию по сектору.
        /// </summary>
        /// <param name="sectorId">Идентификатор сектора.</param>
        /// <returns>Сектор.</returns>
        internal async Task<Sector> GetSectorAsync(int sectorId)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var data = await dataAccessService.GetSectorAsync(sectorId);
            return new Sector(data.Id, data.Name);
        }

        /// <summary>
        /// Добавление сектора.
        /// </summary>
        /// <param name="sector">Сектор.</param>
        internal Task AddSectorAsync(Sector sector)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.AddSectorAsync(new SectorData(sector.Id, sector.Name));
        }

        /// <summary>
        /// Обновление сектора.
        /// </summary>
        /// <param name="sector">Сектор.</param>
        internal Task UpdateSectorAsync(Sector sector)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.UpdateSectorAsync(new SectorData(sector.Id, sector.Name));
        }

        /// <summary>
        /// Сохраняет данные по инструментам по конкретному счету.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные по инструментам.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        internal Task SavePositionTypesData(string accountNumber, PositionTypeData[] data)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.SavePositionTypesDataAsync(accountNumber, data);
        }

        /// <summary>
        /// Сохраняет данные по валютам по конкретному счету.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные по ввалютам.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        internal Task SaveCurrenciesData(string accountNumber, CurrencyData[] data)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.SaveCurrenciesDataAsync(accountNumber, data);
        }

        /// <summary>
        /// Сохраняет данные позиций по конкретному счету.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionType">Тип позиции.</param>
        /// <param name="data">Данные позиций.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        internal Task SavePositionDataAsync(string accountNumber, PositionType positionType, PositionData[] data)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Полученные данные не могут быть неопределенными.");
            }

            var isAnotherType = data.Any(a => a.Type == positionType);
            if (!isAnotherType)
            {
                throw new ApplicationException("Попытка изменить позиции другого типа.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.SavePositionsDataAsync(accountNumber, data);
        }

        /// <summary>
        /// Возвращает настройки.
        /// </summary>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Настройки.</returns>
        internal async Task<IReadOnlyCollection<Option>> GetOptionsAsync(CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var data = await dataAccessService.GetOptionsAsync(cancellation);
            return data.Select(d => new Option(d.Type, d.Value)).ToArray();
        }

        /// <summary>
        /// Обновляет данные настройки.
        /// </summary>
        /// <param name="type">Тип настройки.</param>
        /// <param name="value">Значение.</param>
        /// <param name="cancellation">Токен отмены.</param>
        internal Task UpdateOptionAsync(OptionType type, string value, CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), "Значение настройки не может быть пусстым.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.UpdateOptionAsync(type, value, cancellation);
        }

        /// <summary>
        /// Возвращает значение настройки.
        /// </summary>
        /// <param name="optionType">Тип настройки.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список настроек.</returns>
        internal Task<string?> GetOptionAsync(OptionType optionType, CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.GetOptionAsync(optionType, cancellation);
        }

        /// <summary>
        /// Возвращает данные для экспорта.
        /// </summary>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список настроек.</returns>
        internal async Task<IReadOnlyCollection<AccountExportData>> GetAccountExportDataAsync(CancellationToken cancellation)
        {
            var accountExportDataList = new List<AccountExportData>();
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();

            var positionTypeEnums = Enum.GetValues(typeof(PositionType)).Cast<PositionType>().ToArray();
            var accounts = await dataAccessService.GetAccountDataAsync();
            foreach (var accountNumber in accounts.Select(a => a.Number))
            {
                var accountExportData = new AccountExportData(accountNumber);

                accountExportData.PositionTypes =
                    await GetPositionTypesAsync(dataAccessService, accountNumber, positionTypeEnums);
                accountExportData.Currencies =
                    await GetCurrenciesAsync(dataAccessService, accountNumber);

                accountExportDataList.Add(accountExportData);
            }

            return accountExportDataList.ToArray();
        }

        private static async Task<PositionTypeExportData[]> GetPositionTypesAsync(IDataStorageAccessService dataAccessService, 
            string accountNumber, PositionType[] positionTypeEnums)
        {
            var positionTypes = await dataAccessService.GetPositionTypesAsync(accountNumber, positionTypeEnums);
            var positionTypeExportDataList = new List<PositionTypeExportData>();

            foreach (var positionTypeEnum in positionTypeEnums)
            {
                var positionType = positionTypes.FirstOrDefault(pt => pt.Type == positionTypeEnum);
                var positionTypeExportData = new PositionTypeExportData(positionTypeEnum)
                {
                    PlanPercent = positionType?.PlanPercent ?? 0
                };
                positionTypeExportData.Positions = 
                    await GetPositionsAsync(dataAccessService, accountNumber, positionTypeEnum);

                positionTypeExportDataList.Add(positionTypeExportData);
            }

            return positionTypeExportDataList.ToArray();
        }

        private static async Task<PositionExportData[]> GetPositionsAsync(IDataStorageAccessService dataAccessService, 
            string accountNumber, PositionType positionTypeEnum)
        {
            var positions = await dataAccessService.GetPositionsAsync(accountNumber, positionTypeEnum);
            return positions.Select(p => new PositionExportData(p.Figi)
            {
                PlanPercent = p.PlanPercent
            }).ToArray();
        }

        private async Task<CurrencyExportData[]> GetCurrenciesAsync(IDataStorageAccessService dataAccessService, 
            string accountNumber)
        {
            var currencies = await dataAccessService.GetCurrenciesDataAsync(accountNumber);
            return currencies.Select(p => new CurrencyExportData(p.Currency)
            {
                PlanPercent = p.PlanPercent
            }).ToArray();
        }
    }
}

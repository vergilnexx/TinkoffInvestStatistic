using TinkoffInvestStatistic.Contracts.Enums;
using Domain;
using Infrastructure.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseService : IDataStorageAccessService
    {
        readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            _database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "accounts.db3"));
            _database.CreateTableAsync<AccountData>().Wait();
            _database.CreateTableAsync<CurrencyData>().Wait();
            _database.CreateTableAsync<PositionTypeData>().Wait();
            _database.CreateTableAsync<PositionData>().Wait();
            _database.CreateTableAsync<SectorData>().Wait();
            _database.CreateTableAsync<PlannedPositionData>().Wait();
            _database.CreateTableAsync<OptionData>().Wait();
            _database.CreateTableAsync<TransferBrokerData>().Wait();
            _database.CreateTableAsync<TransferBrokerAccountData>().Wait();

            InitDefaultDataAsync().Wait();
        }

        private async Task InitDefaultDataAsync()
        {
            await InitTransferBrokersDefaultDataAsync();
        }

        private async Task InitTransferBrokersDefaultDataAsync()
        {
            foreach(var brokerName in TransferService.BrokersName)
            {
                var row = await _database.FindAsync<TransferBrokerData>(d => d.BrokerName == brokerName);
                if (row == null)
                {
                    await _database.InsertAsync(new TransferBrokerData() { BrokerName = brokerName });
                }
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<AccountData>> GetAccountDataAsync()
        {
            try
            {
                var data = await _database.Table<AccountData>()
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<PositionData>> GetPositionsAsync(string accountNumber, PositionType positionType)
        {
            try
            {
                var data = await _database.Table<PositionData>()
                                    .Where(p => p.AccountNumber == accountNumber && p.Type == positionType)
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<PositionTypeData>> GetPositionTypesAsync(string accountNumber, PositionType[] positionTypes)
        {
            try
            {
                var data = await _database.Table<PositionTypeData>()
                                    .Where(i => i.AccountNumber == accountNumber && positionTypes.Contains(i.Type))
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<CurrencyData>> GetCurrenciesDataAsync(string accountNumber)
        {
            try
            {
                var data = await _database.Table<CurrencyData>()
                                    .Where(i => i.AccountNumber == accountNumber)
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<SectorData>> GetSectorsAsync()
        {
            try
            {
                var data = await _database.Table<SectorData>()
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<PlannedPositionData>> GetPlannedPositionsAsync(string accountId, PositionType type)
        {
            try
            {
                var data = await _database.Table<PlannedPositionData>()
                                    .Where(p => p.AccountNumber == accountId && p.Type == type)
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<SectorData> GetSectorAsync(int sectorId)
        {
            try
            {
                var data = await _database.Table<SectorData>()
                                    .FirstOrDefaultAsync(s => s.Id == sectorId)
                                    .ConfigureAwait(false);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task AddSectorAsync(SectorData sectorData)
        {
            try
            {
                var sectorEntity = await _database
                                        .Table<SectorData>()
                                        .FirstOrDefaultAsync(sd => sd.Id == sectorData.Id)
                                        .ConfigureAwait(false);
                if (sectorEntity == null)
                {
                    await _database.InsertAsync(sectorData);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task UpdateSectorAsync(SectorData sectorData)
        {
            try
            {
                var sectorEntity = await _database
                                        .Table<SectorData>()
                                        .FirstOrDefaultAsync(sd => sd.Id == sectorData.Id)
                                        .ConfigureAwait(false);
                if (sectorEntity != null)
                {
                    sectorEntity.Name = sectorData.Name;
                    await _database.UpdateAsync(sectorEntity);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task AddPlannedPositionAsync(string accountId, PositionType type, string figi, string name, string ticker)
        {
            try
            {
                var plannedPositionEntity = await _database
                                            .Table<PlannedPositionData>()
                                            .FirstOrDefaultAsync(a => a.Figi == figi)
                                            .ConfigureAwait(false);
                if (plannedPositionEntity == null)
                {
                    await _database.InsertAsync(new PlannedPositionData(accountId, figi, type, name, ticker));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task SaveAccountDataAsync(AccountData[] data)
        {
            try
            {
                foreach (var account in data)
                {
                    var accountEntity = await _database
                                                .Table<AccountData>()
                                                .FirstOrDefaultAsync(a => a.Number == account.Number)
                                                .ConfigureAwait(false);
                    if (accountEntity == null)
                    {
                        await _database.InsertAsync(account);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task SaveCurrenciesDataAsync(string accountNumber, IReadOnlyCollection<CurrencyData> data)
        {
            try
            {
                foreach (var currencyData in data)
                {
                    var currencyEntity = await _database
                                                    .Table<CurrencyData>()
                                                    .Where(pt => pt.AccountNumber == currencyData.AccountNumber)
                                                    .FirstOrDefaultAsync(pt => pt.Currency == currencyData.Currency)
                                                    .ConfigureAwait(false);
                    if (currencyEntity != null)
                    {
                        currencyEntity.PlanPercent = currencyData.PlanPercent;
                        await _database.UpdateAsync(currencyEntity);
                    }
                    else
                    {
                        await _database.InsertAsync(currencyData);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task SavePositionTypesDataAsync(string accountNumber, IReadOnlyCollection<PositionTypeData> data)
        {
            try
            {
                foreach (var positionTypeData in data)
                {
                    var positionTypeEntity = await _database
                                                    .Table<PositionTypeData>()
                                                    .Where(pt => pt.AccountNumber == positionTypeData.AccountNumber)
                                                    .FirstOrDefaultAsync(pt => pt.Type == positionTypeData.Type)
                                                    .ConfigureAwait(false);
                    if (positionTypeEntity != null)
                    {
                        positionTypeEntity.PlanPercent = positionTypeData.PlanPercent;
                        await _database.UpdateAsync(positionTypeEntity);
                    }
                    else
                    {
                        await _database.InsertAsync(positionTypeData);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task SavePositionsDataAsync(string accountNumber, PositionData[] data)
        {
            try
            {
                foreach (var positionTypeData in data)
                {
                    var positionTypeEntity = await _database
                                                    .Table<PositionData>()
                                                    .Where(pt => pt.AccountNumber == positionTypeData.AccountNumber)
                                                    .FirstOrDefaultAsync(pt => pt.Figi == positionTypeData.Figi)
                                                    .ConfigureAwait(false);
                    if (positionTypeEntity != null)
                    {
                        positionTypeEntity.PlanPercent = positionTypeData.PlanPercent;
                        await _database.UpdateAsync(positionTypeEntity);
                    }
                    else
                    {
                        await _database.InsertAsync(positionTypeData);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<OptionData>> GetOptionsAsync(CancellationToken cancellation)
        {
            try
            {
                var options = await _database
                                        .Table<OptionData>()
                                        .ToArrayAsync()
                                        .ConfigureAwait(false);
                return options;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task UpdateOptionAsync(OptionType type, string value, CancellationToken cancellation)
        {
            try
            {
                var option = await _database
                                        .Table<OptionData>()
                                        .FirstOrDefaultAsync(od => od.Type == type)
                                        .ConfigureAwait(false);
                if (option != null)
                {
                    option.Value = value;
                    await _database.UpdateAsync(option);
                }
                else
                {
                    option = new OptionData
                    {
                        Type = type,
                        Value = value
                    };
                    await _database.InsertAsync(option);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<string?> GetOptionAsync(OptionType optionType, CancellationToken cancellation)
        {
            try
            {
                var option = await _database
                                        .Table<OptionData>()
                                        .FirstOrDefaultAsync(o => o.Type == optionType)
                                        .ConfigureAwait(false);
                return option?.Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<TransferBrokerData>> GetTransfersAsync(CancellationToken cancellation)
        {
            try
            {
                var data = await _database
                                    .Table<TransferBrokerData>()
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data ?? Array.Empty<TransferBrokerData>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<TransferBrokerData> GetTransferAsync(string brokerName, CancellationToken cancellation)
        {
            try
            {
                var data = await _database
                                    .Table<TransferBrokerData>()
                                    .FirstOrDefaultAsync(tbd => tbd.BrokerName == brokerName)
                                    .ConfigureAwait(false);
                return data ?? throw new ApplicationException($"Не удалось найти данные по брокеру '{brokerName}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<TransferBrokerAccountData>> GetTransfersBrokerAccountsAsync(int brokerId, CancellationToken cancellation)
        {
            try
            {
                var data = await _database
                                    .Table<TransferBrokerAccountData>()
                                    .Where(tbd => tbd.BrokerId == brokerId)
                                    .ToArrayAsync()
                                    .ConfigureAwait(false);
                return data ?? throw new ApplicationException($"Не удалось найти данные зачислений по счетам брокера №'{brokerId}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
        ///<inheritdoc/>
        public async Task SaveTransferAsync(string brokerName, CancellationToken cancellation)
        {
            try
            {
                var data = await _database
                                        .Table<TransferBrokerData>()
                                        .FirstOrDefaultAsync(tbd => tbd.BrokerName == brokerName)
                                        .ConfigureAwait(false);
                if (data == null)
                {
                    data = new TransferBrokerData
                    {
                        BrokerName = brokerName,
                    };
                    await _database.InsertAsync(data);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task AddTransferBrokerAccountAsync(string brokerName, string name, CancellationToken cancellation)
        {
            try
            {
                var broker = await _database
                                        .Table<TransferBrokerData>()
                                        .FirstOrDefaultAsync(tbd => tbd.BrokerName == brokerName)
                                        .ConfigureAwait(false)
                                        ?? throw new ApplicationException($"Не удалось найти брокера по названию: '{brokerName}'");
                var data = new TransferBrokerAccountData
                {
                    BrokerId = broker.Id,
                    Name = name,
                    Sum = 0m
                };
                await _database.InsertAsync(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task SaveTransferBrokerAccountAsync(int brokerAccountId, decimal sum, CancellationToken cancellation)
        {
            try
            {
                var data = await _database
                                        .Table<TransferBrokerAccountData>()
                                        .FirstOrDefaultAsync(tbad => tbad.Id == brokerAccountId)
                                        .ConfigureAwait(false)
                                        ?? throw new ApplicationException("Не удалось найти счет №" + brokerAccountId);

                data.Sum = sum;
                await _database.UpdateAsync(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}

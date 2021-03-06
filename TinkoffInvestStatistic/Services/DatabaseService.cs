using Contracts;
using Contracts.Enums;
using Domain;
using Infrastructure.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
    }
}

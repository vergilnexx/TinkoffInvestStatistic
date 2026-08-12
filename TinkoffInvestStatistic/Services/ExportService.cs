using Infrastructure.Services;
using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Contracts.Export;
using Microsoft.Maui.Controls;

namespace Services
{
    /// <inheritdoc/>
    public class ExportService : IExportService
    {
        private readonly IFileService _fileservice;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly ISettingService _settingService;

        public ExportService()
        {
            _fileservice = DependencyService.Get<IFileService>();
            _dateTimeProvider = DependencyService.Get<IDateTimeProvider>();

            _settingService = DependencyService.Get<ISettingService>();
        }

        /// <inheritdoc/>
        public async Task ExportAsync(ExportCategories categories, string folder, CancellationToken cancellation)
        {
            await SaveSettingsAsync(categories, folder, cancellation);
            await SaveDataAsync(categories, folder, cancellation);
            await SaveTransfersAsync(categories, folder, cancellation);
        }

        /// <inheritdoc/>
        public async Task ImportAsync(
            ExportCategories categories,
            string folder,
            CancellationToken cancellation,
            string? settingsPath = null,
            string? dataPath = null,
            string? transfersPath = null)
        {
            await ImportSettingsAsync(categories, folder, cancellation, settingsPath);
            await ImportDataAsync(categories, folder, cancellation, dataPath);
            await ImportTransfersAsync(categories, folder, cancellation, transfersPath);
        }

        private async Task SaveSettingsAsync(ExportCategories category, string folder, CancellationToken cancellation)
        {
            if (!category.HasFlag(ExportCategories.Settings))
            {
                return;
            }

            var settings = await _settingService.GetListAsync(cancellation);
            var optionExportData = settings.Select(s => new OptionExportData(s.Type, s.Value)).ToArray();

            await SaveFileAsync(ExportCategories.Settings, optionExportData, folder, cancellation);
        }

        private async Task SaveDataAsync(ExportCategories category, string folder, CancellationToken cancellation)
        {
            if (!category.HasFlag(ExportCategories.Data))
            {
                return;
            }

            var data = await GetAccountExportDataAsync(cancellation);

            await SaveFileAsync(ExportCategories.Data, data, folder, cancellation);
        }

        private async Task SaveTransfersAsync(ExportCategories category, string folder, CancellationToken cancellation)
        {
            if (!category.HasFlag(ExportCategories.Transfers))
            {
                return;
            }

            var transfers = await GetTransfersExportDataAsync(cancellation);

            await SaveFileAsync(ExportCategories.Transfers, transfers, folder, cancellation);
        }

        private async Task SaveFileAsync(ExportCategories category, object data, string folder, CancellationToken cancellation)
        {
            var path = Path.Combine(folder, $"exported_{category}_{_dateTimeProvider.UtcNow:dd.MM.yyyy}.txt");
            await _fileservice.SaveFileAsync(data,  path, cancellation);
        }

        private async Task ImportSettingsAsync(
            ExportCategories categories,
            string folder,
            CancellationToken cancellation,
            string? filePath)
        {
            if (!categories.HasFlag(ExportCategories.Settings))
            {
                return;
            }

            var path = string.IsNullOrWhiteSpace(filePath) ? GetLatestFilePath(ExportCategories.Settings, folder) : filePath;
            if (path == null)
            {
                return;
            }

            var settings = await _fileservice.LoadFileAsync<OptionExportData[]>(path, cancellation);
            foreach (var setting in settings ?? Array.Empty<OptionExportData>())
            {
                if (setting == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(setting.Value))
                {
                    continue;
                }

                await _settingService.UpdateAsync(setting.Type, setting.Value, cancellation);
            }
        }

        private async Task ImportDataAsync(
            ExportCategories categories,
            string folder,
            CancellationToken cancellation,
            string? filePath)
        {
            if (!categories.HasFlag(ExportCategories.Data))
            {
                return;
            }

            var path = string.IsNullOrWhiteSpace(filePath) ? GetLatestFilePath(ExportCategories.Data, folder) : filePath;
            if (path == null)
            {
                return;
            }

            var accounts = await _fileservice.LoadFileAsync<AccountExportData[]>(path, cancellation);
            if (accounts == null || accounts.Length == 0)
            {
                return;
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var accountData = accounts
                .Where(a => a != null)
                .Where(a => !string.IsNullOrEmpty(a.AccountId))
                .Select(a => new AccountData(a.AccountId!))
                .ToArray();
            await dataAccessService.SaveAccountDataAsync(accountData);

            foreach (var account in accounts.Where(a => a != null && !string.IsNullOrEmpty(a.AccountId)))
            {
                var accountId = account.AccountId!;

                var positionTypes = (account.PositionTypes ?? Array.Empty<PositionTypeExportData>())
                    .Select(pt => new PositionTypeData(accountId, pt.Type, pt.PlanPercent))
                    .ToArray();
                await dataAccessService.SavePositionTypesDataAsync(accountId, positionTypes);

                var positions = (account.PositionTypes ?? Array.Empty<PositionTypeExportData>())
                    .SelectMany(pt => pt.Positions ?? Array.Empty<PositionExportData>(),
                        (pt, p) => new { pt.Type, Position = p })
                    .Where(x => !string.IsNullOrEmpty(x.Position.Figi))
                    .Select(x => new PositionData(accountId, x.Position.Figi!, x.Type)
                    {
                        PlanPercent = x.Position.PlanPercent
                    })
                    .ToArray();
                await dataAccessService.SavePositionsDataAsync(accountId, positions);

                var currencies = (account.Currencies ?? Array.Empty<CurrencyExportData>())
                    .Select(c => new CurrencyData(accountId, c.Currency, c.PlanPercent ?? 0m))
                    .ToArray();
                await dataAccessService.SaveCurrenciesDataAsync(accountId, currencies);
            }
        }

        private async Task ImportTransfersAsync(
            ExportCategories categories,
            string folder,
            CancellationToken cancellation,
            string? filePath)
        {
            if (!categories.HasFlag(ExportCategories.Transfers))
            {
                return;
            }

            var path = string.IsNullOrWhiteSpace(filePath) ? GetLatestFilePath(ExportCategories.Transfers, folder) : filePath;
            if (path == null)
            {
                return;
            }

            var transfers = await _fileservice.LoadFileAsync<TransferExportData[]>(path, cancellation);
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            foreach (var transfer in (transfers ?? Array.Empty<TransferExportData>()).Where(t => t != null && !string.IsNullOrEmpty(t.BrokerName)))
            {
                await dataAccessService.SaveTransferAsync(transfer.BrokerName, cancellation);
                var broker = await dataAccessService.GetTransferAsync(transfer.BrokerName, cancellation);
                var accounts = await dataAccessService.GetTransfersBrokerAccountsAsync(broker.Id, cancellation);

                foreach (var account in transfer.AccountData ?? Array.Empty<TransferAccountExportData>())
                {
                    if (account == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(account.Name))
                    {
                        continue;
                    }

                    var existing = accounts.FirstOrDefault(a => a.Name == account.Name);
                    if (existing == null)
                    {
                        await dataAccessService.AddTransferBrokerAccountAsync(transfer.BrokerName, account.Name, cancellation);
                        accounts = await dataAccessService.GetTransfersBrokerAccountsAsync(broker.Id, cancellation);
                        existing = accounts.First(a => a.Name == account.Name);
                    }

                    await dataAccessService.SaveTransferBrokerAccountAsync(existing.Id, account.Sum, cancellation);
                }
            }
        }

        private static string? GetLatestFilePath(ExportCategories category, string folder)
        {
            if (!Directory.Exists(folder))
            {
                return null;
            }

            return Directory
                .GetFiles(folder, $"exported_{category}_*.txt")
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .FirstOrDefault();
        }

        /// <summary>
        /// Возвращает данные для экспорта.
        /// </summary>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список настроек.</returns>
        private async Task<IReadOnlyCollection<AccountExportData>> GetAccountExportDataAsync(CancellationToken cancellation)
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

        /// <summary>
        /// Возвращает данные для экспорта.
        /// </summary>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список настроек.</returns>
        public async Task<IReadOnlyCollection<TransferExportData>> GetTransfersExportDataAsync(CancellationToken cancellation)
        {
            var exportDataList = new List<TransferExportData>();
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();

            var transfers = await dataAccessService.GetTransfersAsync(cancellation);
            foreach (var transfer in transfers)
            {
                var exportData = new TransferExportData(transfer.BrokerName);
                var accountDatas = await dataAccessService.GetTransfersBrokerAccountsAsync(transfer.Id, cancellation);

                exportData.AccountData = accountDatas.Select(ad => new TransferAccountExportData(ad.Name, ad.Sum)).ToArray();

                exportDataList.Add(exportData);
            }

            return exportDataList.ToArray();
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

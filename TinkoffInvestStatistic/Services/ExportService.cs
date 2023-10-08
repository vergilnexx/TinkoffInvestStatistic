using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Contracts.Export;
using Xamarin.Forms;

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

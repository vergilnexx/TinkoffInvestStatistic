using Infrastructure.Services;
using System;
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

            var data = await DataStorageService.Instance.GetAccountExportDataAsync(cancellation);

            await SaveFileAsync(ExportCategories.Data, data, folder, cancellation);
        }

        private async Task SaveTransfersAsync(ExportCategories category, string folder, CancellationToken cancellation)
        {
            if (!category.HasFlag(ExportCategories.Transfers))
            {
                return;
            }

            var transfers = await DataStorageService.Instance.GetTransfersExportDataAsync(cancellation);

            await SaveFileAsync(ExportCategories.Transfers, transfers, folder, cancellation);
        }

        private async Task SaveFileAsync(ExportCategories category, object data, string folder, CancellationToken cancellation)
        {
            var path = Path.Combine(folder, $"exported_{category}_{_dateTimeProvider.UtcNow:dd.MM.yyyy}.txt");
            await _fileservice.SaveFileAsync(data,  path, cancellation);
        }
    }
}

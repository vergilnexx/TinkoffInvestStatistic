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
        public async Task ExportAsync(ExportCategories categories, CancellationToken cancellation)
        {
            await SaveSettingsAsync(categories, cancellation);
            await SaveDataAsync(categories, cancellation);
        }

        private async Task SaveSettingsAsync(ExportCategories category, CancellationToken cancellation)
        {
            if (!category.HasFlag(ExportCategories.Settings))
            {
                return;
            }

            var settings = await _settingService.GetListAsync(cancellation);
            var optionExportData = settings.Select(s => new OptionExportData(s.Type, s.Value)).ToArray();

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(optionExportData.GetType());
            using MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, optionExportData);

            await SaveFileAsync(ExportCategories.Settings, stream, cancellation);
        }

        private async Task SaveDataAsync(ExportCategories category, CancellationToken cancellation)
        {
            if (!category.HasFlag(ExportCategories.Data))
            {
                return;
            }

            var data = await DataStorageService.Instance.GetAccountExportDataAsync(cancellation);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(data.GetType());
            using MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, data);

            await SaveFileAsync(ExportCategories.Data, stream, cancellation);
        }

        private async Task SaveFileAsync(ExportCategories category, MemoryStream stream, CancellationToken cancellation)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                            $"exported_{category}_{_dateTimeProvider.UtcNow:dd.MM.yyyy hh_mm_ss}.xml");
            await _fileservice.SaveFileAsync(stream, path, cancellation);
        }
    }
}

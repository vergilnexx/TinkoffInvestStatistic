using Infrastructure.Services;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Service;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель представления для экспорта.
    /// </summary>
    public class ExportViewModel : BaseViewModel
    {
        /// <summary>
        /// Признак, что надо экспортировать настройки.
        /// </summary>
        public bool IsSettingsExport { get; set; }

        /// <summary>
        /// Признак, что надо экспортировать данные.
        /// </summary>
        public bool IsDataExport { get; set; }

        /// <summary>
        /// Признак, что надо экспортировать зачисления.
        /// </summary>
        public bool IsTransfersExport { get; set; }

        /// <summary>
        /// Команда на экспорт.
        /// </summary>
        public ICommand ExportCommand { get; set; }

        private readonly IExportService _exportService;
        private readonly IFileSystemService _fileSystem;

        public ExportViewModel()
        {
            _exportService = DependencyService.Get<IExportService>();
            _fileSystem = DependencyService.Get<IFileSystemService>();
            ExportCommand = new Command(async() => await ExportAsync());
        }

        /// <summary>
        /// Появление.
        /// </summary>
        public void OnAppearing()
        {
            Title = "Экспорт";
        }

        private async Task ExportAsync()
        {
            IsRefreshing = true;

            try
            {
                var exportCategories = await GetExportCategoriesAsync();
                if (exportCategories == ExportCategories.None)
                {
                    await _messageService.ShowAsync("Необходимо выбрать хотя бы один источник для экспорта.");
                    return;
                }

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                const string folderName = "Documents/tinkoffinveststatistic";
                var folder = _fileSystem.GetExternalStorage(folderName);
                await _exportService.ExportAsync(exportCategories, folder, cancellation);
                await _messageService.ShowAsync("Файлы успешно сохранены в папке " + folderName);
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task<ExportCategories> GetExportCategoriesAsync()
        {
            var result = ExportCategories.None;
            if (IsSettingsExport)
            {
                result |= ExportCategories.Settings;
            }

            if (IsDataExport)
            {
                result |= ExportCategories.Data;
            }

            if (IsTransfersExport)
            {
                var service = DependencyService.Get<IAuthenticateService>();
                var isAuthenticated = await service.AuthenticateAsync("Увидеть зачисления");
                if (!isAuthenticated)
                {
                    throw new ApplicationException("Нет прав на экспорт зачислений.");
                }

                result |= ExportCategories.Transfers;
            }

            return result;
        }
    }
}

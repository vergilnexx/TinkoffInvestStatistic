using Infrastructure.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Service;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Microsoft.Maui.Controls;

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

        /// <summary>
        /// Команда на импорт.
        /// </summary>
        public ICommand ImportCommand { get; set; }

        /// <summary>
        /// Команда выбора файла настроек.
        /// </summary>
        public ICommand PickSettingsFileCommand { get; set; }

        /// <summary>
        /// Команда выбора файла данных.
        /// </summary>
        public ICommand PickDataFileCommand { get; set; }

        /// <summary>
        /// Команда выбора файла зачислений.
        /// </summary>
        public ICommand PickTransfersFileCommand { get; set; }

        private string? _settingsImportPath;
        public string? SettingsImportPath
        {
            get => _settingsImportPath;
            set => SetProperty(ref _settingsImportPath, value);
        }

        private string? _dataImportPath;
        public string? DataImportPath
        {
            get => _dataImportPath;
            set => SetProperty(ref _dataImportPath, value);
        }

        private string? _transfersImportPath;
        public string? TransfersImportPath
        {
            get => _transfersImportPath;
            set => SetProperty(ref _transfersImportPath, value);
        }

        private readonly IExportService _exportService;
        private readonly IFileSystemService? _fileSystem;

        public ExportViewModel()
        {
            _exportService = DependencyService.Get<IExportService>();
            _fileSystem = DependencyService.Get<IFileSystemService>();
            ExportCommand = new Command(async() => await ExportAsync());
            ImportCommand = new Command(async() => await ImportAsync());
            PickSettingsFileCommand = new Command(async () => await PickImportFileAsync(ExportCategories.Settings));
            PickDataFileCommand = new Command(async () => await PickImportFileAsync(ExportCategories.Data));
            PickTransfersFileCommand = new Command(async () => await PickImportFileAsync(ExportCategories.Transfers));
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
                var folder = GetImportExportFolder();
                await _exportService.ExportAsync(exportCategories, folder, cancellation);
                await _messageService.ShowAsync("Файлы успешно сохранены в папке " + folder);
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

        private async Task ImportAsync()
        {
            IsRefreshing = true;

            try
            {
                var importCategories = await GetExportCategoriesAsync();
                if (importCategories == ExportCategories.None)
                {
                    await _messageService.ShowAsync("Необходимо выбрать хотя бы один источник для импорта.");
                    return;
                }

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                var folder = GetImportExportFolder();
                await _exportService.ImportAsync(
                    importCategories,
                    folder,
                    cancellation,
                    SettingsImportPath,
                    DataImportPath,
                    TransfersImportPath);
                await _messageService.ShowAsync("Импорт завершен. Данные восстановлены из доступных файлов.");
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

        private string GetImportExportFolder()
        {
            const string folderName = "Documents/tinkoffinveststatistic";

            if (_fileSystem != null)
            {
                return _fileSystem.GetExternalStorage(folderName);
            }

            var fallbackFolder = Path.Combine(FileSystem.Current.AppDataDirectory, "tinkoffinveststatistic");
            if (!Directory.Exists(fallbackFolder))
            {
                Directory.CreateDirectory(fallbackFolder);
            }

            return fallbackFolder;
        }

        private async Task PickImportFileAsync(ExportCategories category)
        {
            var prefix = category switch
            {
                ExportCategories.Settings => "exported_Settings_",
                ExportCategories.Data => "exported_Data_",
                ExportCategories.Transfers => "exported_Transfers_",
                _ => string.Empty,
            };

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите экспортированный файл",
            });

            if (result == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(result.FullPath))
            {
                await _messageService.ShowAsync("Не удалось получить путь к выбранному файлу.");
                return;
            }

            var fileName = Path.GetFileName(result.FileName ?? string.Empty);
            if (!fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                await _messageService.ShowAsync($"Выбран неверный файл. Ожидается файл вида '{prefix}*.txt'.");
                return;
            }

            switch (category)
            {
                case ExportCategories.Settings:
                    SettingsImportPath = result.FullPath;
                    break;
                case ExportCategories.Data:
                    DataImportPath = result.FullPath;
                    break;
                case ExportCategories.Transfers:
                    TransfersImportPath = result.FullPath;
                    break;
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
                if (service == null)
                {
                    throw new ApplicationException("Сервис аутентификации недоступен.");
                }

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

using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис экспорта.
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Экспорт.
        /// </summary>
        /// <param name="categories">Категории для экспорта.</param>
        /// <param name="folder">Папка для сохранения.</param>
        /// <param name="cancellation">Токен отмены.</param>
        public Task ExportAsync(ExportCategories categories, string folder, CancellationToken cancellation);

        /// <summary>
        /// Импорт ранее экспортированных данных.
        /// </summary>
        /// <param name="categories">Категории для импорта.</param>
        /// <param name="folder">Папка с файлами.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <param name="settingsPath">Явный путь до файла настроек.</param>
        /// <param name="dataPath">Явный путь до файла данных.</param>
        /// <param name="transfersPath">Явный путь до файла зачислений.</param>
        public Task ImportAsync(
            ExportCategories categories,
            string folder,
            CancellationToken cancellation,
            string? settingsPath = null,
            string? dataPath = null,
            string? transfersPath = null);
    }
}

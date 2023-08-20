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
    }
}

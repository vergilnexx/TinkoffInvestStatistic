using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис для работы с файлами.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Сохранение файла в папку.
        /// </summary>
        /// <param name="stream">Поток сохраняемых данных.</param>
        /// <param name="path">Путь к созраняемому файлу.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task SaveFileAsync(Stream stream, string path, CancellationToken cancellation);
    }
}

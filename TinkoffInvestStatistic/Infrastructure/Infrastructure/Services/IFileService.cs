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
        /// <param name="data">Данные для сохранения.</param>
        /// <param name="path">Путь к созраняемому файлу.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task SaveFileAsync(object data, string path, CancellationToken cancellation);

        /// <summary>
        /// Читает и десериализует файл.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Десериализованные данные.</returns>
        Task<T> LoadFileAsync<T>(string path, CancellationToken cancellation);
    }
}

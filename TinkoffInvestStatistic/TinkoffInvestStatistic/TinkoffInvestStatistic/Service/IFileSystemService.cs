namespace TinkoffInvestStatistic.Service
{
    /// <summary>
    /// Интерфейс работы с файловой системой.
    /// </summary>
    public interface IFileSystemService
    {
        /// <summary>
        /// Возвращает путь до внешнего источника.
        /// </summary>
        /// <returns>Путь до внешнего источника.</returns>
        public string GetExternalStorage();
    }
}

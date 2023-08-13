using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис работы с настройками.
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// Возвращает значение настройки.
        /// </summary>
        /// <param name="optionType">Тип настройки.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список настроек.</returns>
        Task<string> GetAsync(OptionType optionType, CancellationToken cancellation);

        /// <summary>
        /// Возвращает список настроек.
        /// </summary>
        /// <param name="cancellation">Токен отмены.</param>
        /// <returns>Список настроек.</returns>
        Task<IReadOnlyCollection<Option>> GetListAsync(CancellationToken cancellation);

        /// <summary>
        /// Обновление настройки.
        /// </summary>
        /// <param name="type">Тип настройки.</param>
        /// <param name="value">Значение.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task UpdateAsync(OptionType type, string value, CancellationToken cancellation);
    }
}

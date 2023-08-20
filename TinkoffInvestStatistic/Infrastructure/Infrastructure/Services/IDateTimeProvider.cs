using System;

namespace Infrastructure.Services
{
    /// <summary>
    /// Провайдер даты и времени.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Текущее дата и время.
        /// </summary>
        public DateTime UtcNow { get; }
    }
}

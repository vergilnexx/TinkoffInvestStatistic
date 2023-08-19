using Infrastructure.Services;
using System;

namespace Services
{
    /// <inheritdoc/>
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc/>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}

using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Services
{
    /// <inheritdoc/>
    public class SettingService : ISettingService
    {
        /// <summary>
        /// Значения настроек по-умолчанию.
        /// </summary>
        private static readonly IDictionary<OptionType, string> DefaultValues = new Dictionary<OptionType, string>()
        {
            { OptionType.IsHideMoney, true.ToString() },
        };

        /// <inheritdoc/>
        public async Task<string> GetAsync(OptionType optionType, CancellationToken cancellation)
        {
            var value = await DataStorageService.Instance.GetOptionAsync(optionType, cancellation);
            if (string.IsNullOrEmpty(value))
            {
                return DefaultValues.FirstOrDefault(dv => dv.Key == optionType).Value;
            }
            return value!;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Option>> GetListAsync(CancellationToken cancellation)
        {
            var list = await DataStorageService.Instance.GetOptionsAsync(cancellation);

            // Заполнение значениями по-умолчанию, если в базе не было данных по настройке.
            var result = from @default in DefaultValues
                            join option in list on @default.Key equals option.Type 
                            into items
                            from item in items.DefaultIfEmpty()
                            select new Option(@default.Key, item?.Value ?? @default.Value);
            return result.ToArray();
        }

        /// <inheritdoc/>
        public Task UpdateAsync(OptionType type, string value, CancellationToken cancellation)
        {
            return DataStorageService.Instance.UpdateOptionAsync(type, value, cancellation);
        }
    }
}

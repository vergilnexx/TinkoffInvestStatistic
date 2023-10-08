using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;
using Xamarin.Forms;

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
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var value = await dataAccessService.GetOptionAsync(optionType, cancellation);
            if (string.IsNullOrEmpty(value))
            {
                return DefaultValues.FirstOrDefault(dv => dv.Key == optionType).Value;
            }
            return value!;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Option>> GetListAsync(CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var data = await dataAccessService.GetOptionsAsync(cancellation);
            var list = data.Select(d => new Option(d.Type, d.Value)).ToArray();

            // Заполнение значениями по-умолчанию, если в базе не было данных по настройке.
            var result = from @default in DefaultValues
                            join option in list on @default.Key equals option.Type 
                            into items
                            from item in items.DefaultIfEmpty()
                            select new Option(@default.Key, item?.Value ?? @default.Value);
            return result.ToArray();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(OptionType type, string value, CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), "Значение настройки не может быть пусстым.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.UpdateOptionAsync(type, value, cancellation);
        }
    }
}

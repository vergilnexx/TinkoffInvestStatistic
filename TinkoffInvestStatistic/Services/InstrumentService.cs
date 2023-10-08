using TinkoffInvestStatistic.Contracts.Enums;
using Domain;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class InstrumentService : IInstrumentService
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Instrument>> GetPositionTypes(string accountNumber)
        {
            var positionTypes = Enum
                                  .GetValues(typeof(PositionType))
                                  .Cast<PositionType>()
                                  .ToArray();

            var filledPositionTypes = await MergePositionTypesDataAsync(accountNumber, positionTypes);

            var accountService = DependencyService.Resolve<IAccountService>();
            var portfolio = await accountService.GetPortfolioAsync(accountNumber);
            foreach (var positionType in filledPositionTypes)
            {
                positionType.Sum = GetSumByPositionType(positionType.Type, portfolio);
            }

            return filledPositionTypes;
        }

        private static decimal GetSumByPositionType(PositionType positionType, Portfolio portfolio)
        {
            var result = positionType switch
            {
                PositionType.Stock => portfolio.TotalAmountStocks?.Sum,
                PositionType.Currency => portfolio.TotalAmountCurrencies?.Sum,
                PositionType.Bond => portfolio.TotalAmountBonds?.Sum,
                PositionType.Etf => portfolio.TotalAmountEtf?.Sum,
                _ => throw new ArgumentOutOfRangeException(nameof(positionType),
                        "Неизвестный тип фин.инструмента: " + positionType)
            };
            return result ?? 0m;
        }

        /// <inheritdoc/>
        public Task SavePlanPercents(string accountNumber, PositionTypeData[] data)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Полученные данные не могут быть неопределенными.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.SavePositionTypesDataAsync(accountNumber, data);
        }

        /// <summary>
        /// Возвращает заполненные данные по инструментам.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionTypes">Типы инструментов.</param>
        /// <returns>Заполненные данные по инструментам</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        private async Task<Instrument[]> MergePositionTypesDataAsync(string accountNumber, PositionType[] positionTypes)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (positionTypes == null || !positionTypes.Any())
            {
                return Array.Empty<Instrument>();
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var instrumentEntities = await dataAccessService.GetPositionTypesAsync(accountNumber, positionTypes);

            var result = new List<Instrument>();
            foreach (var type in positionTypes)
            {
                var instrumentEntity = instrumentEntities.FirstOrDefault(i => i.Type == type);
                if (instrumentEntity == null)
                {
                    instrumentEntity = new PositionTypeData(type);
                }

                result.Add(new Instrument(type, instrumentEntity.PlanPercent));
            }

            return result.ToArray();
        }
    }
}
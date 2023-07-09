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

            var filledPositionTypes = await DataStorageService.Instance.MergePositionTypesData(accountNumber, positionTypes);

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
            return DataStorageService.Instance.SavePositionTypesData(accountNumber, data);
        }
    }
}
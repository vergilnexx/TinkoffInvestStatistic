using Contracts.Enums;
using Microcharts;
using SkiaSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;

namespace TinkoffInvestStatistic.Utility
{
    public sealed class ChartUtility
    {
        public static ChartUtility Instance { get; private set; }

        public ChartUtility()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of ChartUtility is allowed!");
            }
            else
            {
                Instance = this;
            }
        }

        public SKColor GetColor()
        {
            return ChartColorsUtility.Instance.GetColor();
        }

        public async Task<ChartEntry[]> GetChartAsync(AccountsViewModel vm)
        {
            return await GetEntriesAsync(vm);
        }

        public Task<ChartEntry[]> GetChartAsync(PositionTypeViewModel vm)
        {
            return GetEntriesAsync(vm);
        }

        public Task<ChartEntry[]> GetChartAsync(PortfolioViewModel vm)
        {
            return GetEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetEntriesAsync(AccountsViewModel vm)
        {
            return GetAccountEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetAccountEntriesAsync(AccountsViewModel vm)
        {
            var result = vm.Accounts
                            .Select(a => EntryUtility.GetEntry((float)a.CurrentSum, GetColor(), a.AccountType, a.CurrentSumText))
                            .ToArray();
            return Task.FromResult(result);
        }

        private Task<ChartEntry[]> GetEntriesAsync(PositionTypeViewModel vm)
        {
            return GetPositiionTypeEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetPositiionTypeEntriesAsync(PositionTypeViewModel vm)
        {
            var result = vm.PositionTypes
                            .OrderByDescending(t => t.CurrentSum)
                            .Select(t => EntryUtility.GetEntry((float)t.CurrentPercent, GetColor(), t.TypeName, (t.CurrentPercent / 100).ToString("P")))
                            .ToArray();
            return Task.FromResult(result);
        }

        public Task<ChartEntry[]> GetPlannedPositiionTypeEntriesAsync(PositionTypeViewModel vm)
        {
            var result = vm.PositionTypes
                            .OrderByDescending(t => t.CurrentSum)
                            .Select(t => EntryUtility.GetEntry((float)t.PlanPercent, GetColor(), t.TypeName, (t.PlanPercent / 100).ToString("P")))
                            .ToArray();
            return Task.FromResult(result);
        }

        private Task<ChartEntry[]> GetEntriesAsync(PortfolioViewModel vm)
        {
            return GetPositionEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetPositionEntriesAsync(PortfolioViewModel vm)
        {
            var result = vm.GroupedPositions
                            .Where(g => g.Type == (PositionType)vm.PositionType)
                            .FirstOrDefault()
                            .Select(p => EntryUtility.GetEntry((float)p.Sum, GetColor(), p.Name, CurrencyUtility.ToCurrencyString(p.Sum, Currency.Rub)))
                            .OrderByDescending(p => p.Value)
                            .ToArray();
            return Task.FromResult(result);
        }
    }
}

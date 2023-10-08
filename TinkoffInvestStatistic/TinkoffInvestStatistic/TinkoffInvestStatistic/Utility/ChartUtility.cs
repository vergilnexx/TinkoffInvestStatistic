using Microcharts;
using SkiaSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Utility
{
    public sealed class ChartUtility
    {
        public SKColor GetColor()
        {
            var chartColorsUtility = DependencyService.Resolve<ChartColorsUtility>();
            return chartColorsUtility.GetColor();
        }

        public async Task<ChartEntry[]> GetChartAsync(TransferViewModel vm)
        {
            return await GetEntriesAsync(vm);
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

        public async Task<ChartEntry[]> GetChartAsync(CurrencyViewModel vm)
        {
            return await GetEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetEntriesAsync(TransferViewModel vm)
        {
            return GetTransferEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetEntriesAsync(AccountsViewModel vm)
        {
            return GetAccountEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetEntriesAsync(CurrencyViewModel vm)
        {
            return GetCurrenciesEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetEntriesAsync(PositionTypeViewModel vm)
        {
            return GetPositionTypeEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetEntriesAsync(PortfolioViewModel vm)
        {
            return GetPositionEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetTransferEntriesAsync(TransferViewModel vm)
        {
            var result = vm.Brokers
                            .Select(a => EntryUtility.GetEntry((float)a.Sum, GetColor(), a.BrokerName, a.SumText))
                            .ToArray();
            return Task.FromResult(result);
        }

        private Task<ChartEntry[]> GetAccountEntriesAsync(AccountsViewModel vm)
        {
            var result = vm.Accounts
                            .Select(a => EntryUtility.GetEntry((float)a.CurrentSum, GetColor(), a.Name, a.CurrentSumText))
                            .ToArray();
            return Task.FromResult(result);
        }

        private Task<ChartEntry[]> GetCurrenciesEntriesAsync(CurrencyViewModel vm)
        {
            var result = vm.CurrencyTypes
                            .Select(a => EntryUtility.GetEntry((float)a.CurrentSum, GetColor(), a.Name, a.CurrentPercent.ToPercentageString()))
                            .ToArray();
            return Task.FromResult(result);
        }

        public Task<ChartEntry[]> GetPlannedCurrenciesEntriesAsync(CurrencyViewModel vm)
        {
            var result = vm.CurrencyTypes
                            .OrderByDescending(t => t.CurrentSum)
                            .Select(t => EntryUtility.GetEntry((float)t.PlanPercentValue, GetColor(), t.Name, t.PlanPercentValue.ToPercentageString()))
                            .ToArray();
            return Task.FromResult(result);
        }

        private Task<ChartEntry[]> GetPositionTypeEntriesAsync(PositionTypeViewModel vm)
        {
            var result = vm.PositionTypes
                            .OrderByDescending(t => t.CurrentSum)
                            .Select(t => EntryUtility.GetEntry((float)t.CurrentPercent, GetColor(), t.TypeName, t.CurrentPercent.ToPercentageString()))
                            .ToArray();
            return Task.FromResult(result);
        }

        public Task<ChartEntry[]> GetPlannedPositiionTypeEntriesAsync(PositionTypeViewModel vm)
        {
            var result = vm.PositionTypes
                            .OrderByDescending(t => t.CurrentSum)
                            .Select(t => EntryUtility.GetEntry((float)t.PlanPercentValue, GetColor(), t.TypeName, t.PlanPercentValue.ToPercentageString()))
                            .ToArray();
            return Task.FromResult(result);
        }

        private Task<ChartEntry[]> GetPositionEntriesAsync(PortfolioViewModel vm)
        {
            var result = vm.GroupedPositions
                            .FirstOrDefault(g => g.Type == (PositionType)vm.PositionType)
                            .Select(p => EntryUtility.GetEntry((float)p.Sum, GetColor(), p.Name, NumericUtility.ToCurrencyString(p.Sum, Currency.Rub)))
                            .OrderByDescending(p => p.Value)
                            .ToArray();
            return Task.FromResult(result);
        }
    }
}

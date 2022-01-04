using Infrastructure.Helpers;
using Microcharts;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;

namespace TinkoffInvestStatistic.Utility
{
    public sealed class ChartUtility
    {
        public static ChartUtility Instance { get; private set; }

        Random rnd;

        public ChartUtility()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of ChartUtility is allowed!");
            }
            else
            {
                Instance = this;
                RandomGenerator generator = new RandomGenerator();
                this.rnd = generator.GetRandom(null);
            }
        }

        public SKColor GetColor()
        {
            return ChartColors.Instance.GetColor();
        }

        public async Task<Chart> GetChartAsync(AccountsViewModel vm)
        {
            var entries = await GetEntriesAsync(vm);
            return new PieChart() { Entries = entries, HoleRadius = 0.7f, LabelTextSize = 30f, LabelColor=GetColor() };
        }

        public async Task<Chart> GetChartAsync(PortfolioViewModel vm)
        {
            var entries = await GetEntriesAsync(vm);
            return new PieChart() { Entries = entries, HoleRadius = 0.7f, LabelTextSize = 30f, LabelColor = GetColor(), GraphPosition = GraphPosition.AutoFill, LabelMode = LabelMode.None };
        }

        private Task<ChartEntry[]> GetEntriesAsync(AccountsViewModel vm)
        {
            return GetAccountEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetAccountEntriesAsync(AccountsViewModel vm)
        {
            var result = vm.Accounts
                            .Select(a => EntryUtility.GetEntry(int.Parse(a.AccountId), GetColor(), a.AccountType, a.AccountId))
                            .ToArray();
            return Task.FromResult(result);
        }

        private Task<ChartEntry[]> GetEntriesAsync(PortfolioViewModel vm)
        {
            return GetFundEntriesAsync(vm);
        }

        private Task<ChartEntry[]> GetFundEntriesAsync(PortfolioViewModel vm)
        {
            var result = vm.GroupedPositions
                            .Where(g => g.Type == Contracts.Enums.PositionType.Etf)
                            .FirstOrDefault()
                            .Select(p => EntryUtility.GetEntry((float)p.SumInCurrency, GetColor(), p.Name, p.SumInCurrency.ToString("C")))
                            .ToArray();
            return Task.FromResult(result);
        }
    }
}

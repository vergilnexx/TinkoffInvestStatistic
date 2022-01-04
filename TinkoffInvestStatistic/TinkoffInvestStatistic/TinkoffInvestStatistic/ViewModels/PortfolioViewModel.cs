using Infrastructure.Helpers;
using Infrastructure.Services;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Utility;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    [QueryProperty(nameof(AccountTitle), nameof(AccountTitle))]
    public class PortfolioViewModel : BaseViewModel
    {
        private string _accountId;

        public ObservableCollection<GroupedPositionsModel> GroupedPositions { get; }
        public Chart StatisticChart { get; private set; }
        public Command LoadGroupedPositionsCommand { get; }

        public PortfolioViewModel()
        {
            GroupedPositions = new ObservableCollection<GroupedPositionsModel>();
            LoadGroupedPositionsCommand = new Command(async () => await LoadGroupedPositionsByAccountIdAsync());
        }

        public string AccountId
        {
            get
            {
                return _accountId;
            }
            set
            {
                _accountId = value;
            }
        }

        public string AccountTitle
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }

        public async Task<StatisticItem[]> GetStatisticAsync()
        {
            var result = new List<StatisticItem>();
            try
            {
                var service = DependencyService.Get<IPositionService>();
                var grouped = await service.GetGroupedPositionsAsync(AccountId);

                var etf = grouped.FirstOrDefault(x => x.Key == Contracts.Enums.PositionType.Etf);
                foreach (var item in etf.Value)
                {
                    var statisticItem = new StatisticItem();
                    
                    statisticItem.Name = item.Name;
                    statisticItem.SumInRub = item.PositionCount;

                    result.Add(statisticItem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return result.ToArray();
        }

        async Task LoadGroupedPositionsByAccountIdAsync()
        {
            IsBusy = true;

            try
            {
                GroupedPositions.Clear();
                var service = DependencyService.Get<IPositionService>();
                var grouped = await service.GetGroupedPositionsAsync(AccountId);
                foreach (var group in grouped)
                {
                    var items = group.Value.Select(p => new PositionModel
                    {
                        Name = p.Name,
                        Type = p.Type.GetDescription(),
                        PositionCount = p.PositionCount,
                        Blocked = p.Blocked,
                        Ticker = p.Ticker,
                        Currency = p.AveragePositionPrice.Currency,
                        SumInCurrency = p.PositionCount * p.AveragePositionPrice.Value + p.ExpectedYield.Value, // Расчет текущей цены.
                    }).ToList();
                    var model = new GroupedPositionsModel(group.Key, items);

                    GroupedPositions.Add(model);
                }

                await LoadStatisticChartAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadStatisticChartAsync()
        {
            StatisticChart = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}

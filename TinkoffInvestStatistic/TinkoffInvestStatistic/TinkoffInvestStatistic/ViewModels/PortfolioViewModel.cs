using Contracts;
using Contracts.Enums;
using Domain;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Microcharts;
using SkiaSharp;
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
    [QueryProperty(nameof(PositionType), nameof(PositionType))]
    [QueryProperty(nameof(GroupPlanPercent), nameof(GroupPlanPercent))]
    [QueryProperty(nameof(AccountSum), nameof(AccountSum))]
    public class PortfolioViewModel : BaseViewModel
    {
        private string _accountId;
        private PositionType _positionType;

        /// <summary>
        /// Сумма по всем инструментам.
        /// </summary>
        public string Sum { get; private set; }

        /// <summary>
        /// Сумма процентов.
        /// </summary>
        public string SumPercent { get; private set; }

        /// <summary>
        /// Цвет суммы процентов.
        /// </summary>
        public Color SumPercentColor { get; private set; }

        public ObservableCollection<GroupedPositionsModel> GroupedPositions { get; }
        public Chart StatisticChart { get; private set; }
        public Command LoadGroupedPositionsCommand { get; }

        public PortfolioViewModel()
        {
            GroupedPositions = new ObservableCollection<GroupedPositionsModel>();
            LoadGroupedPositionsCommand = new Command(async () => await LoadGroupedPositionsByAccountIdAsync()); 
            StatisticChart = GetChart();
        }

        /// <summary>
        /// Номер счета.
        /// </summary>
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

        /// <summary>
        /// Тип инструментов.
        /// </summary>
        public int PositionType
        {
            get
            {
                return (int)_positionType;
            }
            set
            {
                _positionType = (PositionType)value;
                Title = _positionType.GetDescription();
            }
        }

        /// <summary>
        /// Планируемый процент по группе.
        /// </summary>
        private decimal _groupPlanPercent;
        public decimal GroupPlanPercent
        {
            get
            {
                return _groupPlanPercent;
            }
            set
            {
                _groupPlanPercent = value;
            }
        }

        /// <summary>
        /// Сумма по счету.
        /// </summary>
        private decimal _accountSum;
        public decimal AccountSum
        {
            get
            {
                return _accountSum;
            }
            set
            {
                _accountSum = value;
            }
        }

        private static PieChart GetChart()
        {
            return new PieChart()
            {
                HoleRadius = 0.7f,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Parse("#2B373D"),
                LabelColor = new SKColor(255, 255, 255),
                GraphPosition = GraphPosition.AutoFill,
                LabelMode = LabelMode.None
            };
        }

        async Task LoadGroupedPositionsByAccountIdAsync()
        {
            Sum = SumPercent = string.Empty;
            SumPercentColor = Color.Default;
            IsBusy = true;

            try
            {
                GroupedPositions.Clear();
                var service = DependencyService.Get<IPositionService>();
                var group = (await service.GetGroupedPositionsAsync(AccountId)).FirstOrDefault(g => g.Key == _positionType);

                var sum = AccountSum;
                var models = group.Value
                    .Select(p => new PositionModel(p.Figi, p.Type)
                    {
                        Name = p.Name,
                        PositionCount = p.PositionCount,
                        Blocked = p.Blocked,
                        Ticker = p.Ticker,
                        Currency = p.AveragePositionPrice.Currency,
                        PlanPercent = p.PlanPercent,
                        CurrentPercent = Math.Round(sum == 0 ? 0 : 100 * p.Sum / sum, 2, MidpointRounding.AwayFromZero),
                        Sum = p.Sum,
                        SumInCurrency = p.SumInCurrency,
                        DifferenceSum = p.DifferenceSum,
                        DifferenceSumInCurrency = p.ExpectedYield.Value,
                        DifferenceSumInCurrencyTextColor = p.ExpectedYield.Value > 0 ? Color.Green : Color.Red
                    })
                    .OrderByDescending(p => p.CurrentPercent)
                    .ToList();
                var model = new GroupedPositionsModel(group.Key, models);

                GroupedPositions.Add(model);

                Sum = CurrencyUtility.ToCurrencyString(group.Value.Sum(p => p.Sum), Currency.Rub);
                OnPropertyChanged(nameof(Sum));

                var sumPercent = models.Sum(t => t.PlanPercent);
                SumPercent = (sumPercent / 100).ToString("P");
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetPercentWithoutAllowedDifferenceColor(sumPercent, GroupPlanPercent);
                OnPropertyChanged(nameof(SumPercentColor));

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

        /// <summary>
        /// Сохранение данных.
        /// </summary>
        public async Task SavePlanPercent()
        {
            try
            {
                var service = DependencyService.Get<IPositionService>();
                var data = new List<PositionData>();
                var group = GroupedPositions.FirstOrDefault();
                foreach (var position in group)
                {
                    var item = new PositionData(position.Figi, position.Type);
                    item.PlanPercent = position.PlanPercent;
                    data.Add(item);
                }

                var sumPercent = group.Sum(t => t.PlanPercent);
                SumPercent = (sumPercent / 100).ToString("P");
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetPercentWithoutAllowedDifferenceColor(sumPercent, GroupPlanPercent);
                OnPropertyChanged(nameof(SumPercentColor));

                await service.SavePlanPercents(AccountId, data.ToArray());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task LoadStatisticChartAsync()
        {
            StatisticChart.Entries = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}

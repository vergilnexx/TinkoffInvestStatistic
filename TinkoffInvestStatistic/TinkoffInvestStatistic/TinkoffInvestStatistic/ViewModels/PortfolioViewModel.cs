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
using System.Windows.Input;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.Views;
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
        public PieChart StatisticChart { get; private set; }
        public ICommand LoadGroupedPositionsCommand { get; }

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
                    var item = new PositionData(AccountId, position.Figi, position.Type);
                    item.PlanPercent = position.PlanPercentValue;
                    data.Add(item);
                }

                var sumPercent = group.Sum(t => t.PlanPercentValue);
                SumPercent = (sumPercent / 100).ToString("P");
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetColorPercentWithoutAllowedDifference(sumPercent, GroupPlanPercent);
                OnPropertyChanged(nameof(SumPercentColor));

                await service.SavePlanPercents(AccountId, _positionType, data.ToArray());
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }

        public async Task LoadStatisticChartAsync()
        {
            var entries = await ChartUtility.Instance.GetChartAsync(this);
            StatisticChart.Entries = entries;
            StatisticChart.LabelMode = entries.Length > 5 ? LabelMode.None : LabelMode.RightOnly;
            OnPropertyChanged(nameof(StatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task LoadGroupedPositionsByAccountIdAsync()
        {
            Sum = SumPercent = string.Empty;
            SumPercentColor = Color.Default;
            IsBusy = true;

            try
            {
                GroupedPositions.Clear();
                var service = DependencyService.Get<IPositionService>();
                var data = await service.GetPositionsByTypeAsync(AccountId, _positionType);

                var sum = AccountSum;
                var models = data
                    .Select(p => new PositionModel(p.Figi, p.Type)
                    {
                        Name = p.Name,
                        PositionCount = p.PositionCount,
                        Ticker = p.Ticker,
                        Currency = p.AveragePositionPrice?.Currency ?? Currency.Rub,
                        PlanPercent = p.PlanPercent.ToString(),
                        CurrentPercent = Math.Round(sum == 0 ? 0 : 100 * p.Sum / sum, 2, MidpointRounding.AwayFromZero),
                        Sum = p.Sum,
                        SumInCurrency = p.SumInCurrency,
                        DifferenceSum = p.DifferenceSum,
                        DifferenceSumInCurrency = p.ExpectedYield?.Sum ?? 0,
                        DifferenceSumInCurrencyTextColor = (p.ExpectedYield?.Sum ?? 0) >= 0 ? Color.Green : Color.Red,
                        IsBlocked = p.IsBlocked
                    })
                    .OrderByDescending(p => p.CurrentPercent)
                    .ToList();
                var model = new GroupedPositionsModel(_positionType, models);

                GroupedPositions.Add(model);

                Sum = CurrencyUtility.ToCurrencyString(data.Sum(p => p.Sum), Currency.Rub);
                OnPropertyChanged(nameof(Sum));

                var sumPercent = models.Sum(t => t.PlanPercentValue);
                SumPercent = (sumPercent / 100).ToString("P");
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetColorPercentWithoutAllowedDifference(sumPercent, GroupPlanPercent);
                OnPropertyChanged(nameof(SumPercentColor));

                await LoadStatisticChartAsync();
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private PieChart GetChart()
        {
            return new PieChart()
            {
                HoleRadius = 0.6f,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Parse("#2B373D"),
                LabelColor = new SKColor(255, 255, 255),
                GraphPosition = GraphPosition.AutoFill,
                LabelMode = LabelMode.None
            };
        }
    }
}

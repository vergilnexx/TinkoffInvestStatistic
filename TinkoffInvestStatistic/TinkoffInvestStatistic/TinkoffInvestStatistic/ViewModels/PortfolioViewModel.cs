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
using TinkoffInvestStatistic.ViewModels.Base;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    [QueryProperty(nameof(PositionType), nameof(PositionType))]
    [QueryProperty(nameof(GroupPlanPercent), nameof(GroupPlanPercent))]
    [QueryProperty(nameof(AccountSum), nameof(AccountSum))]
    public class PortfolioViewModel : BaseViewModel
    {
        public PortfolioViewModel()
        {
            GroupedPositions = new ObservableCollection<GroupedPositionsModel>();
            LoadGroupedPositionsCommand = new Command(async () => await LoadGroupedPositionsByAccountIdAsync());
            StatisticChart = GetChart();
        }

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

        /// <summary>
        /// Данные сгруппированных позиций.
        /// </summary>
        public ObservableCollection<GroupedPositionsModel> GroupedPositions { get; }

        /// <summary>
        /// Диаграмма статистики.
        /// </summary>
        public PieChart StatisticChart { get; private set; }

        /// <summary>
        /// Команда на загрузку сгруппированных позиций.
        /// </summary>
        public ICommand LoadGroupedPositionsCommand { get; }

        /// <summary>
        /// Номер счета.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal GroupPlanPercent { get; set; }

        /// <summary>
        /// Сумма счета.
        /// </summary>
        public decimal AccountSum { get; set; }

        /// <summary>
        /// Тип инструментов.
        /// </summary>
        private PositionType _positionType;

        public int PositionType
        {
            get => (int)_positionType;
            set
            {
                _positionType = (PositionType)value;
                Title = _positionType.GetDescription();
            }
        }

        /// <summary>
        /// Сохранение данных.
        /// </summary>
        public async Task SavePlanPercentAsync()
        {
            try
            {
                var service = DependencyService.Get<IPositionService>();
                var data = new List<PositionData>();
                var group = GroupedPositions.FirstOrDefault() ??
                            new GroupedPositionsModel(_positionType, new List<PositionModel>());
                foreach (var position in group)
                {
                    var item = new PositionData(AccountId, position.Figi, position.Type);
                    item.PlanPercent = position.PlanPercentValue;
                    data.Add(item);
                }

                var sumPercent = group.Sum(t => t.PlanPercentValue);
                SumPercent = sumPercent.ToPercentageString();
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor =
                    DifferencePercentUtility.GetColorPercentWithoutAllowedDifference(sumPercent, GroupPlanPercent);
                OnPropertyChanged(nameof(SumPercentColor));

                await service.SavePlanPercents(AccountId, _positionType, data.ToArray());
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Событие появления.
        /// </summary>
        public void OnAppearing()
        {
            IsRefreshing = true;
        }

        /// <summary>
        /// Загрузка диаграммы статистики.
        /// </summary>
        /// <returns></returns>
        private async Task LoadStatisticChartAsync()
        {
            var entries = await ChartUtility.Instance.GetChartAsync(this);
            StatisticChart.Entries = entries;
            StatisticChart.LabelMode = entries.Length > 5 ? LabelMode.None : LabelMode.RightOnly;
            OnPropertyChanged(nameof(StatisticChart));
        }

        private async Task LoadGroupedPositionsByAccountIdAsync()
        {
            Sum = SumPercent = string.Empty;
            SumPercentColor = Color.Default;
            IsRefreshing = true;

            try
            {
                GroupedPositions.Clear();
                var models = await GetPositionDataAsync();
                var model = new GroupedPositionsModel(_positionType, models);

                GroupedPositions.Add(model);

                Sum = GetViewMoney(() => models.Sum(p => p.Sum).ToCurrencyString(Currency.Rub));
                OnPropertyChanged(nameof(Sum));

                var sumPercent = models.Sum(t => t.PlanPercentValue);
                SumPercent = sumPercent.ToPercentageString();
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor =
                    DifferencePercentUtility.GetColorPercentWithoutAllowedDifference(sumPercent, GroupPlanPercent);
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
                IsRefreshing = false;
            }
        }

        private async Task<List<PositionModel>> GetPositionDataAsync()
        {
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
                    CurrentPercent = NumericUtility.ToPercentage(sum, p.Sum),
                    Sum = p.Sum,
                    SumInCurrency = p.SumInCurrency,
                    DifferenceSum = p.DifferenceSum,
                    DifferenceSumInCurrency = p.ExpectedYield?.Sum ?? 0,
                    DifferenceSumInCurrencyTextColor = (p.ExpectedYield?.Sum ?? 0) >= 0 ? Color.Green : Color.Red,
                    IsBlocked = p.IsBlocked,
                    SumText = GetViewMoney(() =>
                        p.Currency != Currency.Rub
                            ? $" / {p.Sum.ToCurrencyString(Currency.Rub)}"
                            : string.Empty),
                    SumInCurrencyText =
                        GetViewMoney(() => p.SumInCurrency.ToCurrencyString(p.Currency)),
                    DifferenceSumText = GetViewMoney(() =>
                        p.Currency != Currency.Rub
                            ? $" / {p.DifferenceSum.ToCurrencyString(Currency.Rub)}"
                            : string.Empty),
                    DifferenceSumInCurrencyText = GetViewMoney(() =>
                        (p.ExpectedYield?.Sum ?? 0).ToCurrencyString(p.Currency)),
                })
                .OrderByDescending(p => p.CurrentPercent)
                .ToList();
            return models;
        }

        private static PieChart GetChart()
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
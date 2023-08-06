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
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель типов инструменто (ETF, акции, облигации)
    /// </summary>
    public class PositionTypeViewModel : BaseViewModel
    {
        public string AccountId { get; set; }

        private PositionTypeModel _selectedItem;

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

        public ObservableCollection<PositionTypeModel> PositionTypes { get; }
        public Chart StatisticChart { get; private set; }
        public Chart PlannedStatisticChart { get; private set; }
        public Command LoadPositionTypesCommand { get; }
        public Command<PositionTypeModel> ItemTapped { get; }

        public PositionTypeViewModel()
        {
            PositionTypes = new ObservableCollection<PositionTypeModel>();
            LoadPositionTypesCommand = new Command(async () => await ExecuteLoadPositionTypesCommandAsync());
            ItemTapped = new Command<PositionTypeModel>(OnPositionTypeSelected);
            StatisticChart = GetChart();
            PlannedStatisticChart = GetChart();
        }

        private static PieChart GetChart()
        {
            return new PieChart()
            {
                HoleRadius = 0.6f,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Parse("#2B373D"),
                LabelColor = new SKColor(255, 255, 255),
                LabelMode = LabelMode.RightOnly
            };
        }

        private async Task ExecuteLoadPositionTypesCommandAsync()
        {
            Sum = SumPercent = string.Empty;
            SumPercentColor = Color.Default;
            IsBusy = true;

            try
            {
                PositionTypes.Clear();

                var service = DependencyService.Get<IInstrumentService>();
                IEnumerable<Instrument> positionTypes = await service.GetPositionTypes(AccountId);
                positionTypes = positionTypes.OrderByDescending(t => t.Sum);
                var sum = positionTypes.Sum(t => t.Sum);
                foreach (var item in positionTypes)
                {
                    var model = new PositionTypeModel(item.Type);
                    model.PlanPercent = item.PlanPercent.ToString();
                    model.CurrentPercent = NumericUtility.ToPercentage(sum, item.Sum);
                    model.CurrentSum = item.Sum;
                    model.CurrentSumText = GetViewMoney(() => NumericUtility.ToCurrencyString(item.Sum, Currency.Rub));

                    PositionTypes.Add(model);
                }
                
                Sum = GetViewMoney(() => NumericUtility.ToCurrencyString(sum, Currency.Rub));
                OnPropertyChanged(nameof(Sum));

                var sumPercent = positionTypes.Sum(t => t.PlanPercent);
                SumPercent = sumPercent.ToPercentageString();
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetColorPercentWithoutAllowedDifference(sumPercent, 100);
                OnPropertyChanged(nameof(SumPercentColor));

                await LoadStatisticChartAsync();
                await LoadPlannedStatisticChartAsync();
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

        /// <summary>
        /// Сохранение данных.
        /// </summary>
        public async Task SavePlanPercentAsync()
        {
            try
            {
                var service = DependencyService.Get<IInstrumentService>();
                var data = PositionTypes.Select(pt => new PositionTypeData(AccountId, pt.Type, pt.PlanPercentValue));

                var sumPercent = PositionTypes.Sum(t => t.PlanPercentValue);
                SumPercent = sumPercent.ToPercentageString();
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetColorPercentWithoutAllowedDifference(sumPercent, 100);
                OnPropertyChanged(nameof(SumPercentColor));

                await service.SavePlanPercents(AccountId, data.ToArray());
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }

        public async Task LoadStatisticChartAsync()
        {
            StatisticChart.Entries = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        public async Task LoadPlannedStatisticChartAsync()
        {
            PlannedStatisticChart.Entries = await ChartUtility.Instance.GetPlannedPositiionTypeEntriesAsync(this);
            OnPropertyChanged(nameof(PlannedStatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
            Title = "Инструменты";
        }

        public PositionTypeModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnPositionTypeSelected(value);
            }
        }

        private async void OnPositionTypeSelected(PositionTypeModel item)
        {
            if (item == null)
            {
                return;
            }

            var sum = PositionTypes.Sum(t => t.CurrentSum);
            var url = $"{nameof(PortfolioPage)}" +
                $"?{nameof(PortfolioViewModel.AccountId)}={AccountId}" +
                $"&{nameof(PortfolioViewModel.PositionType)}={(int)item.Type}" +
                $"&{nameof(PortfolioViewModel.GroupPlanPercent)}={item.PlanPercent}" +
                $"&{nameof(PortfolioViewModel.AccountSum)}={Math.Round(sum, DecimalHelper.NUMERIC_DECIMALS, MidpointRounding.ToEven)}";
            await Shell.Current.GoToAsync(url, true);
        }
    }
}

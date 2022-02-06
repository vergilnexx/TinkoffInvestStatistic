using Contracts;
using Contracts.Enums;
using Domain;
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
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель типов инструменто (ETF, акции, облигации)
    /// </summary>
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    public class PositionTypeViewModel : BaseViewModel
    {
        private string _accountId;
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

        public PositionTypeViewModel()
        {
            PositionTypes = new ObservableCollection<PositionTypeModel>();
            LoadPositionTypesCommand = new Command(async () => await ExecuteLoadPositionTypesCommand());
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
        private async Task ExecuteLoadPositionTypesCommand()
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
                    model.PlanPercent = item.PlanPercent;
                    model.CurrentPercent = Math.Round(sum == 0 ? 0 : 100 * item.Sum / sum, 2, MidpointRounding.AwayFromZero);
                    model.CurrentSum = item.Sum;

                    PositionTypes.Add(model);
                }
                Sum = CurrencyUtility.ToCurrencyString(sum, Currency.Rub);
                OnPropertyChanged(nameof(Sum));

                var sumPercent = positionTypes.Sum(t => t.PlanPercent);
                SumPercent = (sumPercent / 100).ToString("P");
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetPercentWithoutAllowedDifferenceColor(sumPercent, 100);
                OnPropertyChanged(nameof(SumPercentColor));

                await LoadStatisticChartAsync();
                await LoadPlannedStatisticChartAsync();
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
                var service = DependencyService.Get<IInstrumentService>();
                var data = new List<InstrumentData>();
                foreach (var positionTypeItem in PositionTypes)
                {
                    var item = new InstrumentData(positionTypeItem.Type);
                    item.PlanPercent = positionTypeItem.PlanPercent;
                    data.Add(item);
                }

                var sumPercent = PositionTypes.Sum(t => t.PlanPercent);
                SumPercent = (sumPercent / 100).ToString("P");
                OnPropertyChanged(nameof(SumPercent));

                SumPercentColor = DifferencePercentUtility.GetPercentWithoutAllowedDifferenceColor(sumPercent, 100);
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

        public async Task LoadPlannedStatisticChartAsync()
        {
            PlannedStatisticChart.Entries = await ChartUtility.Instance.GetPlannedPositiionTypeEntriesAsync(this);
            OnPropertyChanged(nameof(PlannedStatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
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

        async void OnPositionTypeSelected(PositionTypeModel item)
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
                $"&{nameof(PortfolioViewModel.AccountSum)}={Math.Round(sum, 2, MidpointRounding.ToEven)}";
            await Shell.Current.GoToAsync(url, true);
        }
    }
}

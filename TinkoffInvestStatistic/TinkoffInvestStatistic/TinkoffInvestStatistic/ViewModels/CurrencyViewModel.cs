using Contracts.Enums;
using Infrastructure.Services;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Utility;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель представления статистики по валютам.
    /// </summary>
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    public class CurrencyViewModel : BaseViewModel
    {
        private string _accountId;

        /// <summary>
        /// Номер счета.
        /// </summary>
        public string AccountId
        {
            get { return _accountId;}
            set { _accountId = value; }
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

        public ObservableCollection<CurrencyTypeModel> CurrencyTypes { get; }
        public Chart StatisticChart { get; private set; }
        public Chart PlannedStatisticChart { get; private set; }
        public Command LoadCurrenciesCommand { get; }

        public CurrencyViewModel()
        {
            CurrencyTypes = new ObservableCollection<CurrencyTypeModel>();
            LoadCurrenciesCommand = new Command(async () => await ExecuteLoadCurrenciesCommand());
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

        private async Task ExecuteLoadCurrenciesCommand()
        {
            Sum = SumPercent = string.Empty;
            SumPercentColor = Color.Default;
            IsBusy = true;

            try
            {
                CurrencyTypes.Clear();
                var service = DependencyService.Get<IAccountService>();
                var currencies = await service.GetAccountDataByCurrenciesTypes(AccountId);
                currencies = currencies.OrderByDescending(t => t.Sum).ToArray();
                var sum = currencies.Sum(t => t.Sum);
                foreach (var item in currencies)
                {
                    var currentPercent = Math.Round(sum == 0 ? 0 : 100 * item.Sum / sum, 2, MidpointRounding.AwayFromZero);
                    var model = new CurrencyTypeModel(item.Currency, item.Sum, item.PlanPercent, currentPercent);

                    CurrencyTypes.Add(model);
                }
                Sum = CurrencyUtility.ToCurrencyString(sum, Currency.Rub);
                OnPropertyChanged(nameof(Sum));

                var sumPercent = currencies.Sum(t => t.PlanPercent);
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

        public async Task LoadStatisticChartAsync()
        {
            StatisticChart.Entries = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        public async Task LoadPlannedStatisticChartAsync()
        {
            PlannedStatisticChart.Entries = await ChartUtility.Instance.GetPlannedCurrenciesEntriesAsync(this);
            OnPropertyChanged(nameof(PlannedStatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        public void SavePlanPercent()
        {
            try
            {
                //var service = DependencyService.Get<IInstrumentService>();
                //var data = new List<InstrumentData>();
                //foreach (var positionTypeItem in PositionTypes)
                //{
                //    var item = new InstrumentData(positionTypeItem.Type);
                //    item.PlanPercent = positionTypeItem.PlanPercent;
                //    data.Add(item);
                //}

                //var sumPercent = PositionTypes.Sum(t => t.PlanPercent);
                //SumPercent = (sumPercent / 100).ToString("P");
                //OnPropertyChanged(nameof(SumPercent));

                //SumPercentColor = DifferencePercentUtility.GetPercentWithoutAllowedDifferenceColor(sumPercent, 100);
                //OnPropertyChanged(nameof(SumPercentColor));

                //await service.SavePlanPercents(AccountId, data.ToArray());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}

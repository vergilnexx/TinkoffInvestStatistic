using Domain;
using Infrastructure.Services;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.ViewModels.Base;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель представления статистики по валютам.
    /// </summary>
    public class CurrencyViewModel : BaseViewModel
    {
        public string AccountId { get; set; }

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
                LabelTextSize = 23f,
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
                    var currentPercent = NumericUtility.ToPercentage(sum, item.Sum);
                    var model = new CurrencyTypeModel(item.Currency, item.Sum, item.PlanPercent.ToString(), currentPercent);
                    model.CurrentSumText = GetViewMoney(() => NumericUtility.ToCurrencyString(item.Sum, Currency.Rub));
                    CurrencyTypes.Add(model);
                }
                Sum = GetViewMoney(() => NumericUtility.ToCurrencyString(sum, Currency.Rub));
                OnPropertyChanged(nameof(Sum));

                var sumPercent = currencies.Sum(t => t.PlanPercent);
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

        public async Task SavePlanPercent()
        {
            try
            {
                var service = DependencyService.Get<ICurrencyService>();
                var data = CurrencyTypes.Select(c => new CurrencyData(AccountId, c.Currency, c.PlanPercentValue));

                var sumPercent = CurrencyTypes.Sum(t => t.PlanPercentValue);
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
    }
}

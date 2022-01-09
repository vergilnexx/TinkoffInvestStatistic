using Contracts.Enums;
using Domain;
using Infrastructure.Services;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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

        public ObservableCollection<PositionTypeModel> PositionTypes { get; }
        public Chart StatisticChart { get; private set; }
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
        }

        private async Task ExecuteLoadPositionTypesCommand()
        {
            Sum = SumPercent = string.Empty;
            IsBusy = true;

            try
            {
                PositionTypes.Clear();
                var service = DependencyService.Get<IInstrumentService>();
                IEnumerable<InstrumentData> positionTypes = await service.GetPositionTypes(AccountId);
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
                Sum = sum.ToString("C", CultureInfo.GetCultureInfo("ru-RU"));
                OnPropertyChanged(nameof(Sum));

                SumPercent = (positionTypes.Sum(t => t.PlanPercent) / 100).ToString("P");
                OnPropertyChanged(nameof(SumPercent));

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
        public Task SavePlanPercent()
        {
            var service = DependencyService.Get<IInstrumentService>();
            var data = new List<InstrumentData>();
            foreach (var positionTypeItem in PositionTypes)
            {
                var item = new InstrumentData(positionTypeItem.Type);
                item.PlanPercent = positionTypeItem.PlanPercent;
                data.Add(item);
            }

            SumPercent = (PositionTypes.Sum(t => t.PlanPercent) / 100).ToString("P");
            OnPropertyChanged(nameof(SumPercent));

            return service.SavePlanPercents(AccountId, data.ToArray());
        }

        public async Task LoadStatisticChartAsync()
        {
            StatisticChart = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
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
            var url = $"{nameof(PortfolioPage)}" +
                $"?{nameof(PortfolioViewModel.AccountId)}={AccountId}" +
                $"&{nameof(PortfolioViewModel.PositionType)}={(int)item.Type}";
            await Shell.Current.GoToAsync(url, true);
        }
    }
}

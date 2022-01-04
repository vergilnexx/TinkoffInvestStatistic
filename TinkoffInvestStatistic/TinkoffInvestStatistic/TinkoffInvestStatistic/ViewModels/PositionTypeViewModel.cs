using Contracts.Enums;
using Microcharts;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Models;
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
            IsBusy = true;

            try
            {
                PositionTypes.Clear();
                var positionTypes = Enum.GetValues(typeof(PositionType)).Cast<PositionType>();
                foreach (var item in positionTypes)
                {
                    var model = new PositionTypeModel(item);

                    PositionTypes.Add(model);
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
            //StatisticChart = await ChartUtility.Instance.GetChartAsync(this);
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

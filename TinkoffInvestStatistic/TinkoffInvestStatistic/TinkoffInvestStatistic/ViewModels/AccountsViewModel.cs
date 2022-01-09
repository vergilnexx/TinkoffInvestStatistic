using Infrastructure.Helpers;
using Infrastructure.Services;
using Microcharts;
using SkiaSharp;
using System;
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
    public class AccountsViewModel : BaseViewModel
    {
        private AccountModel _selectedItem;

        /// <summary>
        /// Сумма по всем инструментам.
        /// </summary>
        public string Sum { get; private set; }

        public ObservableCollection<AccountModel> Accounts { get; }
        public Chart StatisticChart { get; private set; }
        public Command LoadAccountsCommand { get; }
        public Command<AccountModel> ItemTapped { get; }

        public AccountsViewModel()
        {
            Title = "Счета";
            Accounts = new ObservableCollection<AccountModel>();
            StatisticChart = GetChart();
            LoadAccountsCommand = new Command(async () => await ExecuteLoadAccountsCommand());
            ItemTapped = new Command<AccountModel>(OnAccountSelected);
        }

        private static PieChart GetChart()
        {
            return new PieChart()
            {
                HoleRadius = 0.7f,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Parse("#2B373D"),
                LabelColor = new SKColor(255,255,255),
            };
        }

        private async Task ExecuteLoadAccountsCommand()
        {
            Sum = string.Empty;
            IsBusy = true;

            try
            {
                Accounts.Clear();
                var service = DependencyService.Get<IAccountService>();
                var accounts = await service.GetAccountsAsync();
                foreach (var item in accounts)
                {
                    var model = new AccountModel(item.ID, item.Type.GetDescription());
                    model.CurrentSum = item.Sum;

                    Accounts.Add(model);
                }

                var sum = accounts.Sum(t => t.Sum);
                Sum = CurrencyUtility.ToCurrencyString(sum, Contracts.Enums.Currency.Rub);
                OnPropertyChanged(nameof(Sum));

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
            StatisticChart.Entries = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public AccountModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnAccountSelected(value);
            }
        }

        async void OnAccountSelected(AccountModel item)
        {
            if (item == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(PositionTypesPage)}" +
                $"?{nameof(PositionTypeViewModel.AccountId)}={item.AccountId}", true);
        }
    }
}
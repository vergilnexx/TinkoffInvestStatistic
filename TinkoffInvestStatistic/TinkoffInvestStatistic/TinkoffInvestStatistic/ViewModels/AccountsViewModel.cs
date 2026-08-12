using Infrastructure.Helpers;
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
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Microsoft.Maui.Controls;

namespace TinkoffInvestStatistic.ViewModels
{
    public class AccountsViewModel : BaseViewModel
    {
        private bool _isNavigatingToAccount;

        public AccountsViewModel()
        {
            Title = "Счета";
            Accounts = new ObservableCollection<AccountModel>();
            StatisticChart = GetChart();
            LoadAccountsCommand = new Command(async () => await ExecuteLoadAccountsCommandAsync());
            ItemTapped = new Command<AccountModel>(OnAccountSelected);
        }

        private AccountModel _selectedItem;

        /// <summary>
        /// Сумма по всем инструментам.
        /// </summary>
        public string Sum { get; private set; }

        /// <summary>
        /// Счета.
        /// </summary>
        public ObservableCollection<AccountModel> Accounts { get; }

        /// <summary>
        /// Диаграммы статистики.
        /// </summary>
        public PieChart StatisticChart { get; private set; }

        /// <summary>
        /// Команда на загрузку данных о счетах.
        /// </summary>
        public Command LoadAccountsCommand { get; }

        /// <summary>
        /// Команда выбора счета.
        /// </summary>
        public Command<AccountModel> ItemTapped { get; }

        /// <summary>
        /// Выбранный счет.
        /// </summary>
        public AccountModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnAccountSelected(value);
            }
        }

        /// <summary>
        /// Событие появления.
        /// </summary>
        public void OnAppearing()
        {
            IsRefreshing = true;
            SelectedItem = null;
        }

        /// <summary>
        /// Загрузка диаграммы статустики
        /// </summary>
        /// <returns></returns>
        public async Task LoadStatisticChartAsync()
        {
            var chartUtility = DependencyService.Resolve<ChartUtility>();
            if (chartUtility == null)
            {
                return;
            }
            StatisticChart.Entries = await chartUtility.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        private static PieChart GetChart()
        {
            return new PieChart()
            {
                HoleRadius = 0.6f,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Parse("#2B373D"),
                LabelColor = new SKColor(255,255,255),
            };
        }

        /// <summary>
        /// Выполнение команды загрузки информации о счетах.
        /// </summary>
        private async Task ExecuteLoadAccountsCommandAsync()
        {
            Sum = string.Empty;
            IsRefreshing = true;

            try
            {
                Accounts.Clear();
                var service = DependencyService.Get<IAccountService>();
                if (service == null)
                {
                    return;
                }
                var accounts = await service.GetAccountsAsync();
                var sum = accounts.Sum(a => a.Sum);
                foreach (var item in accounts)
                {
                    var model = new AccountModel(item.ID, item.Name, item.Type.GetDescription(), item.Sum);
                    model.CurrentSumText = IsShowMoney()
                                                ? NumericUtility.ToCurrencyString(item.Sum, Contracts.Enums.Currency.Rub)
                                                : NumericUtility.ToPercentageString(sum, item.Sum);
                    Accounts.Add(model);
                }
                
                Sum = GetViewMoney(() => NumericUtility.ToCurrencyString(accounts.Sum(t => t.Sum), Contracts.Enums.Currency.Rub));
                OnPropertyChanged(nameof(Sum));

                await LoadStatisticChartAsync();
            }
            catch (Exception ex)
            {
                if (_messageService != null)
                {
                    await _messageService.ShowAsync(ex.Message);
                }

                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Событие выбора счета.
        /// </summary>
        /// <param name="item">Данные выбранного счета</param>
        private async void OnAccountSelected(AccountModel item)
        {
            if (item == null || _isNavigatingToAccount)
            {
                return;
            }

            try
            {
                _isNavigatingToAccount = true;

                var accountId = Uri.EscapeDataString(item.AccountId ?? string.Empty);
                var accountName = Uri.EscapeDataString(item.Name ?? string.Empty);
                var url = $"{nameof(PositionTypesPage)}" +
                            $"?{nameof(AccountStatisticViewModel.AccountId)}={accountId}" +
                            $"&{nameof(AccountStatisticViewModel.AccountName)}={accountName}";

                await Shell.Current.GoToAsync(url, animate: true);
            }
            catch (Exception ex)
            {
                if (_messageService != null)
                {
                    await _messageService.ShowAsync(ex.Message);
                }

                Debug.WriteLine(ex);
            }
            finally
            {
                _isNavigatingToAccount = false;
            }
        }
    }
}
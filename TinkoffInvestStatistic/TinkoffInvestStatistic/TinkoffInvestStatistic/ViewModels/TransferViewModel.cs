using Infrastructure.Services;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TinkoffInvest.Contracts.Accounts;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Service;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Данные представления страницы зачислений.
    /// </summary>
    public class TransferViewModel : BaseViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal TransferViewModel()
        {
            Brokers = new ObservableCollection<TransferBrokerModel>();
            LoadCommand = new Command(async () => await LoadAsync());
            SaveCommand = new Command(async() => await SaveAsync());
            AddBrokerAccountCommand = new Command<TransferBrokerModel>(AddBrokerAccountAsync);
            StatisticChart = GetChart();
        }

        /// <summary>
        /// Диаграммы статистики.
        /// </summary>
        public Chart StatisticChart { get; private set; }

        /// <summary>
        /// Данные о зачислениях по брокерам.
        /// </summary>
        public ObservableCollection<TransferBrokerModel> Brokers { get; private set; }

        /// <summary>
        /// Команда на сохранение.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Команда на загрузку.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Команда на добавление брокерского счета.
        /// </summary>
        public Command<TransferBrokerModel> AddBrokerAccountCommand { get; }

        /// <summary>
        /// Сумма по всем счетам и брокерам.
        /// </summary>
        public string Sum { get; private set; }

        public async Task OnAppearing()
        {
            Title = "Зачисления";

            //var service = DependencyService.Get<IAuthenticateService>();
            //var isAuthenticated = await service.AuthenticateAsync("Увидеть зачисления");
            //if (!isAuthenticated)
            //{
            //    IsRefreshing = false;
            //    await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
            //    return;
            //}

            IsRefreshing = true;
        }

        public void OnDisappearing()
        {
            Brokers.Clear();
            StatisticChart.Entries = Array.Empty<ChartEntry>();
        }

        /// <summary>
        /// Загрузка диаграммы статустики
        /// </summary>
        /// <returns></returns>
        public async Task LoadStatisticChartAsync()
        {
            StatisticChart.Entries = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        private static PieChart GetChart()
        {
            return new PieChart()
            {
                HoleRadius = 0.6f,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Parse("#2B373D"),
                LabelColor = new SKColor(255, 255, 255),
            };
        }

        private async Task SaveAsync()
        {
            try
            {
                var service = DependencyService.Get<ITransferService>();

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                foreach (var broker in Brokers)
                {
                    var amounts = broker.AccountData.Select(ad => new TransferBrokerAccount(ad.Name, ad.Amount)).ToArray();
                    await service.SaveAsync(broker.BrokerName, amounts, cancellation);
                }
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }

            // Перезагружаем данные.
            IsRefreshing = true;
        }

        private async Task LoadAsync()
        {
            IsRefreshing = true;
            Brokers.Clear();

            try
            {
                var service = DependencyService.Get<ITransferService>();

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                var brokers = await service.GetListAsync(cancellation);
                foreach (var broker in brokers)
                {
                    var data = new TransferBrokerModel(broker.BrokerName);
                    data.Sum = broker.AccountData.Sum(ad => ad.Sum);
                    data.SumText = NumericUtility.ToCurrencyString(data.Sum, Contracts.Enums.Currency.Rub);
                    data.AccountData = broker.AccountData.Select(ad => new TransferBrokerAccountModel(ad.Name, ad.Sum)).ToArray();
                    Brokers.Add(data);
                }

                Sum = NumericUtility.ToCurrencyString(Brokers.Sum(b => b.Sum), Contracts.Enums.Currency.Rub);
                OnPropertyChanged(nameof(Sum));

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

        private async void AddBrokerAccountAsync(TransferBrokerModel data)
        {
            try
            {
                string brokerName = data.BrokerName;

                var name = await _messageService.ShowPromptAsync("Добавление счета", "Брокер: " + brokerName);
                if (name == null)
                {
                    return;
                }

                if (name.Length > 200)
                {
                    await _messageService.ShowAsync("Название счета не должно быть больше 200 символов.");
                    return;
                }


                var service = DependencyService.Get<ITransferService>();
                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                await service.AddTransferBrokerAccountAsync(brokerName, name, cancellation);

                // Перезагружаем данные.
                IsRefreshing = true;
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }
    }
}

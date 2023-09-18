﻿using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinkoffInvestStatistic.ViewModels.Base;
using Xamarin.Forms;
using XCalendar.Core.Models;
using Infrastructure.Services;
using TinkoffInvestStatistic.Models;
using System.Threading;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Contracts;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Helpers;
using XCalendar.Core.Extensions;
using TinkoffInvest.Contracts.Accounts;
using XCalendar.Core.Collections;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Данные представления страницы уведомлений по зачислениям.
    /// </summary>
    public class TransferNotificationViewModel : BaseViewModel
    {
        public Calendar<CalendarDay> Calendar { get; set; } = new Calendar<CalendarDay>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal TransferNotificationViewModel()
        {
            NotificationPeriodData = new ObservableCollection<TransferNotificationModel>();

            LoadCommand = new Command(async () => await LoadAsync());
            AddDateCommand = new Command(async () => await AddDateAsync());
            DeleteDateCommand = new Command<TransferNotificationModel>(DeleteDateAsync);
            SaveCommand = new Command(async () => await SaveDateAsync());
            NavigateCalendarCommand = new Command<int>(NavigateCalendar);
        }

        /// <summary>
        /// Команда навигации по календарю.
        /// </summary>
        public ICommand NavigateCalendarCommand { get; set; }

        /// <summary>
        /// Команда на загрузку.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Команда на добавление уведомления.
        /// </summary>
        public ICommand AddDateCommand { get; }

        /// <summary>
        /// Команда на удаления уведомления.
        /// </summary>
        public ICommand DeleteDateCommand { get; }

        /// <summary>
        /// Команда на сохранение уведомлений.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Данные сгруппированных позиций.
        /// </summary>
        public ObservableCollection<TransferNotificationModel> NotificationPeriodData { get; }

        /// <summary>
        /// Событие появления.
        /// </summary>
        public Task OnAppearing()
        {
            Title = "Уведомления";

            var dateTimeProvider = DependencyService.Get<IDateTimeProvider>();
            Calendar.Navigate(dateTimeProvider.UtcNow - Calendar.NavigatedDate);

            IsRefreshing = true;

            return Task.CompletedTask;
        }

        private void NavigateCalendar(int amount)
        {
            //Months are variable length, calculate the timespan needed to get to the result.
            DateTime targetDateTime = Calendar.NavigatedDate.AddMonths(amount);

            Calendar.Navigate(targetDateTime - Calendar.NavigatedDate);
            Calendar.SelectedDates?.Clear();

            foreach (var notificationDate in NotificationPeriodData)
            {
                AddNotificationPeriod(Calendar.SelectedDates, Calendar.NavigatedDate, notificationDate);
            }
        }

        private void AddNotificationPeriod(ObservableRangeCollection<DateTime> selectedDates, 
            DateTime currentDate, TransferNotificationModel notificationDate)
        {
            DateTime? startDate = notificationDate.StartDate;
            while (startDate?.Year == currentDate.Year)
            {
                selectedDates.Add(startDate.Value);
                startDate = CalculateDate(notificationDate.PeriodType, startDate.Value, amount: 1);
            }
        }

        private static DateTime? CalculateDate(TransferNotificationPeriodType periodType, DateTime startDate, int amount)
        {
            DateTime? date = periodType switch
            {
                TransferNotificationPeriodType.Week => startDate.AddWeeks(amount),
                TransferNotificationPeriodType.Month => startDate.AddMonths(amount),
                TransferNotificationPeriodType.Quarter => startDate.AddMonths(3 * amount),
                TransferNotificationPeriodType.Year => startDate.AddYears(amount),
                TransferNotificationPeriodType.None => null,
                _ => null,
            };
            return date;
        }

        private async Task LoadAsync()
        {
            IsRefreshing = true;
            NotificationPeriodData?.Clear();

            try
            {
                var service = DependencyService.Get<ITransferNotificationService>();
                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                var notifications = await service.GetListAsync(cancellation);
                foreach (var notification in notifications)
                {
                    NotificationPeriodData.Add(new TransferNotificationModel(notification.Id, notification.StartDate, notification.PeriodType));
                }
                NavigateCalendar(default);
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

        private async Task AddDateAsync()
        {
            try
            {
                var dateTimeProvider = DependencyService.Get<IDateTimeProvider>();
            
                // По-умолчанию создаём на каждыый месяц в 10:00
                var data = new TransferNotificationDto(dateTimeProvider.UtcNow.Date.AddHours(10), TransferNotificationPeriodType.Month);

                var service = DependencyService.Get<ITransferNotificationService>();
                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                await service.AddAsync(data, cancellation);

                // Перезагружаем данные.
                IsRefreshing = true;
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }

        private async void DeleteDateAsync(TransferNotificationModel data)
        {
            try
            {
                var service = DependencyService.Get<ITransferNotificationService>();
                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                await service.DeleteAsync(data.Id, cancellation);

                // Перезагружаем данные.
                IsRefreshing = true;
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }

        private async Task SaveDateAsync()
        {
            try
            {
                var notifications = NotificationPeriodData
                                        .Select(n => new TransferNotificationDto(n.Id, n.StartDate.Add(n.Time), EnumHelper.GetTransferNotificationPeriodType(n.PeriodTypeText)))
                                        .ToArray();

                var service = DependencyService.Get<ITransferNotificationService>();
                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                await service.SaveAsync(notifications, cancellation);

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

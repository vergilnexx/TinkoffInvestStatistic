using System.Threading.Tasks;
using System.Threading;
using Plugin.LocalNotification;
using TinkoffInvestStatistic.Droid.Services;
using Xamarin.Forms;
using TinkoffInvestStatistic.Contracts;
using System;
using TinkoffInvestStatistic.Contracts.Enums;

[assembly: Dependency(typeof(NotificationService))]
namespace TinkoffInvestStatistic.Droid.Services
{
    /// <inheritdoc/>
    public class NotificationService : Service.INotificationService
    {
        /// <inheritdoc/>
        public async Task AddRequestAsync(int notificationId, TransferNotificationDto data, CancellationToken cancellation)
        {
            if (await  LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

            if (data.PeriodType == PeriodDatesType.None)
            {
                return;
            }

            if (notificationId <= 0)
            {
                return;
            }

            var notification = new NotificationRequest
            {
                NotificationId = notificationId,
                Title = "Уведомление",
                Description = "Необходимо пополнить счёт",
                Schedule = new NotificationRequestSchedule()
                {
                    NotifyTime = data.StartDate,
                    RepeatType = NotificationRepeat.TimeInterval,
                    NotifyRepeatInterval = GetNotifyRepeatInterval(data.PeriodType)
                }
            };
            await LocalNotificationCenter.Current.Show(notification);
        }

        public Task ChangeAsync(int notificationId, TransferNotificationDto notification, CancellationToken cancellation)
        {
            Delete(notificationId);
            return AddRequestAsync(notificationId, notification, cancellation);
        }

        public void Delete(int notificationId)
        {
            LocalNotificationCenter.Current.Cancel(notificationId);
        }

        private TimeSpan? GetNotifyRepeatInterval(PeriodDatesType periodType)
        {
            var now = DateTime.Now;
            return periodType switch
            {
                PeriodDatesType.Week => TimeSpan.FromDays(7),
                PeriodDatesType.Month => now.AddMonths(1) - now,
                PeriodDatesType.Quarter => now.AddMonths(3) - now,
                PeriodDatesType.Year => now.AddYears(1) - now,
                _ => null
            };
        }
    }
}
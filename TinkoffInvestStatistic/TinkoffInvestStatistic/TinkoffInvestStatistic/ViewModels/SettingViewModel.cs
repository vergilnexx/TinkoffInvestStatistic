using Infrastructure.Services;
using System.Diagnostics;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.ViewModels.Base;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Представление настроек.
    /// </summary>
    public class SettingViewModel : BaseViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SettingViewModel()
        {
            LoadOptionsCommand = new Command(async () => await ExecuteLoadOptionsCommandAsync());
        }

        /// <summary>
        /// Команда на загрузку настроек.
        /// </summary>
        public Command LoadOptionsCommand { get; }

        /// <summary>
        /// Скрывать данные при входе.
        /// </summary>
        public bool IsHideMoney { get; set; }

        public void OnAppearing()
        {
            IsRefreshing = true;
            Title = "Настройки";
        }

        private async Task ExecuteLoadOptionsCommandAsync()
        {
            IsRefreshing = true;

            try
            {
                var service = DependencyService.Get<ISettingService>();

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                var options = await service.GetListAsync(cancellation);

                IsHideMoney = options.FirstOrDefault(o => o.Type == OptionType.IsHideMoney)?.ToBoolean() ?? true;
                OnPropertyChanged(nameof(IsHideMoney));
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

        /// <summary>
        /// Сохранение значения настройки.
        /// </summary>
        /// <param name="optionType">Тип настрйоки.</param>
        /// <param name="value">Значение.</param>
        public async Task SaveOptionAsync(OptionType optionType, string value)
        {
            try
            {
                var service = DependencyService.Get<ISettingService>();

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                await service.UpdateAsync(optionType, value, cancellation);
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }
    }
}

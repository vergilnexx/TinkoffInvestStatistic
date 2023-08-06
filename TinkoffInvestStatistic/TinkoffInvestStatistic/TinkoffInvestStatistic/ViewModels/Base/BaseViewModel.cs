using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TinkoffInvestStatistic.Service;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly IMessageService _messageService;

        private readonly IHideShowMoneyService _hideShowMoneyService;
        private string title = string.Empty;
        private readonly static string HidedValue = "***";

        public BaseViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _hideShowMoneyService = DependencyService.Get<IHideShowMoneyService>();
        }

        /// <summary>
        /// Признак, что приложение занято обновлением данных.
        /// </summary>
        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        /// <summary>
        /// Возвращает признак видимости данных.
        /// </summary>
        /// <returns>Признак видимости данных.</returns>
        public bool IsShowMoney()
        {
            return _hideShowMoneyService.IsShow();
        }

        /// <summary>
        /// Возвращает строковое значнеие суммы денег для отображения.
        /// </summary>
        /// <param name="action">Расчет строкового значения.</param>
        /// <returns>Строковое значнеие суммы денег для отображения.</returns>
        public string GetViewMoney(Func<string> action)
        {
            if (!IsShowMoney())
            {
                return HidedValue;
            }

            return action();
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        /// <summary>
        /// Событие измеенения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

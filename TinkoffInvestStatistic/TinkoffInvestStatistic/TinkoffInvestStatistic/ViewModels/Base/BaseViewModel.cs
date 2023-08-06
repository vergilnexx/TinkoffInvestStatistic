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

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        protected readonly static string HidedValue = "***";

        public BaseViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _hideShowMoneyService = DependencyService.Get<IHideShowMoneyService>();
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
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

        /// <summary>
        /// Возвращает признак видимости данных.
        /// </summary>
        /// <returns>Признак видимости данных.</returns>
        public bool IsShowMoney()
        {
            return _hideShowMoneyService.IsShow();
        }

        public string GetViewMoney(Func<string> action)
        {
            if (!IsShowMoney())
            {
                return HidedValue;
            }

            return action();
        }

        #region INotifyPropertyChanged

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

using TinkoffInvestStatistic.ViewModels.Base;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель представления статистики по счету.
    /// </summary>
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    [QueryProperty(nameof(AccountName), nameof(AccountName))]
    public class AccountStatisticViewModel : BaseViewModel
    {

        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Наименование счета.
        /// </summary>
        private string _accountName;
        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                Title = _accountName;
            }
        }
    }
}

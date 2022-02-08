using System.Collections.Generic;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель представления статистики по счету.
    /// </summary>
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    public class AccountStatisticViewModel : BaseViewModel
    {
        public string AccountId { get; set; }
    }
}

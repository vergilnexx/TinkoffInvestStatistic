using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountStatisticPage : BaseTabbedDataPage
    {
        AccountStatisticViewModel _viewModel = new AccountStatisticViewModel();

        public AccountStatisticPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;
        }
    }
}
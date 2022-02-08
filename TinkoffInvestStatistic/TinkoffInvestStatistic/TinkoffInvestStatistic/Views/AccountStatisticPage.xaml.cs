using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountStatisticPage : TabbedPage
    {
        AccountStatisticViewModel _viewModel = new AccountStatisticViewModel();

        public AccountStatisticPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;
        }
    }
}
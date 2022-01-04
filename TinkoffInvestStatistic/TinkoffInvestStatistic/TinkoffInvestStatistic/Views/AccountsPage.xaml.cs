using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    public partial class AccountsPage : ContentPage
    {
        AccountsViewModel _viewModel = DependencyService.Resolve<AccountsViewModel>();

        public AccountsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel = null;
        }
    }
}
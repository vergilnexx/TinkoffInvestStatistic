using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;

namespace TinkoffInvestStatistic.Views
{
    /// <summary>
    /// Страница счетов.
    /// </summary>
    public partial class AccountsPage : BaseDataPage
    {
        AccountsViewModel _viewModel = new AccountsViewModel();

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
    }
}
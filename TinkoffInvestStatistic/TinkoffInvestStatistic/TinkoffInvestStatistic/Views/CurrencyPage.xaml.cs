using System;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencyPage : BaseDataPage
    {
        CurrencyViewModel _viewModel;

        public CurrencyPage()
        {
            InitializeComponent();
            _viewModel = new CurrencyViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var t = Application.Current.MainPage;
            var page = t.Navigation.NavigationStack.Last();
            var mainPage = page.BindingContext as AccountStatisticViewModel;
            _viewModel.AccountId = mainPage.AccountId;
            _viewModel.OnAppearing();
        }

        private void PlanPercent_Completed(object sender, System.EventArgs e)
        {
            Task.Run(() => _viewModel.SavePlanPercent());
        }
    }
}
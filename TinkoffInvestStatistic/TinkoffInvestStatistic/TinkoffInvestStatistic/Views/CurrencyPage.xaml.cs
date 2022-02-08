using System;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencyPage : ContentPage
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void PlanPercent_Completed(object sender, System.EventArgs e)
        {
            Task.Run(() => _viewModel.SavePlanPercent());
        }
    }
}
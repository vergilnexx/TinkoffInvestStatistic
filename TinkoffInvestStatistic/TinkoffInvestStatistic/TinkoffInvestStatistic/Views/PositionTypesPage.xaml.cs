using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionTypesPage : BaseDataPage
    {
        PositionTypeViewModel _viewModel;

        public PositionTypesPage()
        {
            InitializeComponent();

            _viewModel = new PositionTypeViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var mainPage = Application.Current.MainPage;
            var page = mainPage.Navigation.NavigationStack.Last();
            var model = page.BindingContext as AccountStatisticViewModel;
            _viewModel.AccountId = model.AccountId;
            _viewModel.OnAppearing();
        }

        private void PlanPercent_Completed(object sender, System.EventArgs e)
        {
            Task.Run(() => _viewModel.SavePlanPercentAsync());
        }
    }
}
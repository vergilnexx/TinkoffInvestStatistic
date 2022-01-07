using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionTypesPage : ContentPage
    {
        PositionTypeViewModel _viewModel = new PositionTypeViewModel();

        public PositionTypesPage()
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
        }

        private void PlanPercent_Completed(object sender, System.EventArgs e)
        {
            Task.Run(() => _viewModel.SavePlanPercent());
        }
    }
}
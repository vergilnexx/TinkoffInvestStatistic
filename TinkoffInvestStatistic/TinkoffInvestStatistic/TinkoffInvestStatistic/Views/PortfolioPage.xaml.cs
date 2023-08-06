using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    public partial class PortfolioPage : BaseDataPage
    {
        PortfolioViewModel _viewModel = new PortfolioViewModel();

        public PortfolioPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;
        }

        /// <inheritdoc/>
        public override void RefreshView()
        {
            _viewModel.IsRefreshing = true;
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void PlanPercent_Completed(object sender, System.EventArgs e)
        {
            Task.Run(() => _viewModel.SavePlanPercentAsync());
        }
    }
}
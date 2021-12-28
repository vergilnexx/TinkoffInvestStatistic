using Microcharts;
using SkiaSharp;
using System.ComponentModel;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    public partial class PortfolioPage : ContentPage
    {
        PortfolioViewModel _viewModel = DependencyService.Resolve<PortfolioViewModel>();

        public PortfolioPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;

            var entries = new []
            {
                new ChartEntry(-100)
                {
                    Label = "March",
                    Color = SKColor.Parse("#00BFFF")
                },
                new ChartEntry(400)
                {
                    Label = "February",
                    Color = SKColor.Parse("#00CED1")
                }
            };
            Chart.Chart = new PieChart { Entries = entries };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
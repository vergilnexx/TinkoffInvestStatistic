using System.ComponentModel;
using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    /// <summary>
    /// Страница с табами о зачислениях.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferTabbedPage : BaseTabbedDataPage
    {
        TransferTabbedViewModel _viewModel = new TransferTabbedViewModel();

        public TransferTabbedPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;
        }

        /// <inheritdoc/>
        public override void RefreshView()
        {
            var currentTab = CurrentPage as BaseDataPage;
            currentTab?.RefreshView();
        }
    }
}
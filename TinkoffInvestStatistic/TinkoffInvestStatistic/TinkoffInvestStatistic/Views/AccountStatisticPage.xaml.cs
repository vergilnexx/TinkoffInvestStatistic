using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Linq;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountStatisticPage : BaseTabbedDataPage
    {
        AccountStatisticViewModel _viewModel = new AccountStatisticViewModel();

        public AccountStatisticPage()
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplyAccountContext();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            ApplyAccountContext();
        }

        private void ApplyAccountContext()
        {
            if (BindingContext is not AccountStatisticViewModel model)
            {
                return;
            }

            foreach (var page in Children.OfType<Page>())
            {
                switch (page)
                {
                    case PositionTypesPage positionTypesPage:
                        positionTypesPage.SetAccountId(model.AccountId);
                        break;
                    case CurrencyPage currencyPage:
                        currencyPage.SetAccountId(model.AccountId);
                        break;
                }
            }
        }
    }
}
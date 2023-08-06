using System;
using TinkoffInvestStatistic.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views.Base
{
    /// <summary>
    /// Базовая страница с табами.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public abstract partial class BaseTabbedDataPage : TabbedPage
    {
        private readonly IHideShowMoneyService _hideShowMoneyService;

        protected BaseTabbedDataPage() : base()
        {
            _hideShowMoneyService = DependencyService.Get<IHideShowMoneyService>();

            Init();
        }

        private void Init()
        {

            var toolbarItem = new ToolbarItem() { IconImageSource = _hideShowMoneyService.GetIconFileName(), Priority = 0, Order = ToolbarItemOrder.Primary };
            toolbarItem.Clicked += OnItemClicked;
            this.ToolbarItems.Add(toolbarItem);
        }

        private void OnItemClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;

            var isShow = _hideShowMoneyService.IsShow();
            _hideShowMoneyService.SetShow(!isShow);
            var icon = _hideShowMoneyService.GetIconFileName();
            item.IconImageSource = icon;
        }
    }
}
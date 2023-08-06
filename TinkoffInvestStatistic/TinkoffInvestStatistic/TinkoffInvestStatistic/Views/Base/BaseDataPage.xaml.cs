using System;
using TinkoffInvestStatistic.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views.Base
{
    /// <summary>
    /// Базовая страница.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public abstract partial class BaseDataPage : ContentPage
    {
        private readonly IHideShowMoneyService _hideShowMoneyService;

        protected BaseDataPage() : base()
        {
            _hideShowMoneyService = DependencyService.Get<IHideShowMoneyService>();

            Init();
        }

        /// <summary>
        /// Обновление данных.
        /// </summary>
        public abstract void RefreshView();

        private void Init()
        {
            var toolbarItem = new ToolbarItem() { IconImageSource = _hideShowMoneyService.GetIconFileName(), Priority = 0, Order = ToolbarItemOrder.Primary };
            toolbarItem.Clicked += OnShowHideMoneyClicked;
            this.ToolbarItems.Add(toolbarItem);
        }

        private void OnShowHideMoneyClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;

            var isShow = _hideShowMoneyService.IsShow();
            _hideShowMoneyService.SetShow(!isShow);
            var icon = _hideShowMoneyService.GetIconFileName();
            item.IconImageSource = icon;

            RefreshView();
        }
    }
}
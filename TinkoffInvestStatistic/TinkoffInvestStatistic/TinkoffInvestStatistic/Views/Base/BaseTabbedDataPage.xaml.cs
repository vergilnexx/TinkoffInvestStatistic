using Plugin.Fingerprint;
using System;
using System.Threading.Tasks;
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

        private async void OnShowHideMoneyClicked(object sender, EventArgs e)
        {
            var isShow = _hideShowMoneyService.IsShow();
            if (!isShow && !(await _hideShowMoneyService.IsAvailableShowAsync()))
            {
                return;
            }

            ToolbarItem item = (ToolbarItem)sender;

            _hideShowMoneyService.SetShow(!isShow);
            var icon = _hideShowMoneyService.GetIconFileName();
            item.IconImageSource = icon;

            RefreshView();
        }
    }
}
namespace TinkoffInvestStatistic.Service
{
    /// <inheritdoc/>
    public class HideShowMoneyService : IHideShowMoneyService
    {
        /// <summary>
        /// Признак, что можно показывать суммы.
        /// </summary>
        private static bool IsShowMoney { get; set; } = true;

        private readonly static string IconHideFileName = "icon_hide.png";
        private readonly static string IconShowFileName = "icon_show.png";

        /// <inheritdoc/>
        public string GetIconFileName()
        {
            return IsShowMoney
                    ? IconShowFileName
                    : IconHideFileName;
        }

        /// <inheritdoc/>
        public bool IsShow()
        {
            return IsShowMoney;
        }

        /// <inheritdoc/>
        public void SetShow(bool isShow)
        {
            IsShowMoney = isShow;
        }
    }
}

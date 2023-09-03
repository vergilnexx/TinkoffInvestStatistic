using System.Threading.Tasks;

namespace TinkoffInvestStatistic.Service
{
    /// <inheritdoc/>
    public class MessageService : IMessageService
    {
        /// <inheritdoc/>
        public Task ShowAsync(string message)
        {
            return App.Current.MainPage.DisplayAlert("TinkoffInvestStatistic.App", message, "Ok");
        }

        /// <inheritdoc/>
        public Task<string> ShowPromptAsync(string message)
        {
            return App.Current.MainPage.DisplayPromptAsync("TinkoffInvestStatistic.App", message, "Ok");
        }
    }
}

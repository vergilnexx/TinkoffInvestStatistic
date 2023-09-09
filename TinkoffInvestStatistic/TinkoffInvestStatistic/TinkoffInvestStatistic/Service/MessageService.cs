using System.Threading.Tasks;

namespace TinkoffInvestStatistic.Service
{
    /// <inheritdoc/>
    public class MessageService : IMessageService
    {
        /// <inheritdoc/>
        public Task ShowAsync(string message)
        {
            return App.Current.MainPage.DisplayAlert("Внимание!", message, "Ok");
        }

        /// <inheritdoc/>
        public Task<string> ShowPromptAsync(string title, string message)
        {
            return App.Current.MainPage.DisplayPromptAsync(title, message, "Ok");
        }
    }
}

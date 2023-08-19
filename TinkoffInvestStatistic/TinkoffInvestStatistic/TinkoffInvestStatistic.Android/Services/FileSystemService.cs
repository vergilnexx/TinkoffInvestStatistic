using TinkoffInvestStatistic.Droid.Services;
using TinkoffInvestStatistic.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSystemService))]
namespace TinkoffInvestStatistic.Droid.Services
{
    /// <inheritdoc/>
    public class FileSystemService : IFileSystemService
    {
        /// <inheritdoc/>
        public string GetExternalStorage()
        {
            var context = Android.App.Application.Context;
            var filePath = context.GetExternalFilesDir(string.Empty);
            return filePath.Path;
        }
    }
}
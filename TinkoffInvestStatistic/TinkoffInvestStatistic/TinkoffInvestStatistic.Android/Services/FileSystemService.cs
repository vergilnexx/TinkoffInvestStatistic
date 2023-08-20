using Android.Content;
using System;
using System.IO;
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
        public string GetExternalStorage(string folderName)
        {
            string externalStorageDirectory = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string documentsDirectory = Path.Combine(externalStorageDirectory, folderName);
            if (!File.Exists(documentsDirectory))
            {
                Directory.CreateDirectory(documentsDirectory);
            }
            return documentsDirectory;
        }
    }
}
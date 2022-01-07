using Domain;
using Infrastructure.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Файловый сервис.
    /// </summary>
    public class FileService : IDataStorageAccessService
    {
        private static readonly string AccountFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/TinkoffInvestStatisticData";

        /// <inheritdoc/>
        public async Task<AccountData[]> GetAccountDataAsync()
        {
            var data = await ReadDataFromFile(AccountFilePath);
            return JsonConvert.DeserializeObject<AccountData[]>(data) ?? Array.Empty<AccountData>();
        }

        /// <inheritdoc/>
        public Task SaveAccountDataAsync(AccountData[] data)
        {
            var json = JsonConvert.SerializeObject(data);
            return WriteDataToFile(AccountFilePath, json);
        }

        private async Task<string> ReadDataFromFile(string filePath) 
        {
            using var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true);

            var sb = new StringBuilder();

            byte[] buffer = new byte[0x1000];
            int numRead;
            while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                sb.Append(text);
            }

            return sb.ToString();
        }

        private async Task WriteDataToFile(string filePath, string data)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(data);

            using var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.OpenOrCreate, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true);

            await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        }
    }
}

using Domain;
using Infrastructure.Services;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
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
            try
            {
                var data = await ReadDataFromFile(AccountFilePath);
                return JsonConvert.DeserializeObject<AccountData[]>(data) ?? Array.Empty<AccountData>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        /// <inheritdoc/>
        public async Task SaveAccountDataAsync(AccountData[] data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                await WriteDataToFile(AccountFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
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
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true);

            await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        }
    }
}

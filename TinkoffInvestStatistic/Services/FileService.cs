using Infrastructure.Services;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services
{
    /// <inheritdoc/>
    public class FileService : IFileService
    {
        /// <inheritdoc/>
        public async Task SaveFileAsync(object data, string path, CancellationToken cancellation)
        {
            var  serializer = new XmlSerializer(data.GetType());

            using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream, Encoding.UTF8);
            
            serializer.Serialize(writer, data);
            await writer.FlushAsync();
        }

        /// <inheritdoc/>
        public async Task<T> LoadFileAsync<T>(string path, CancellationToken cancellation)
        {
            var serializer = new XmlSerializer(typeof(T));

            await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(fileStream, Encoding.UTF8);

            var data = serializer.Deserialize(reader);
            if (data is not T typedData)
            {
                throw new ApplicationException($"Не удалось прочитать файл: {path}");
            }

            return typedData;
        }
    }
}

using Infrastructure.Services;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    /// <inheritdoc/>
    public class FileService : IFileService
    {
        /// <inheritdoc/>
        public async Task SaveFileAsync(Stream stream, string path, CancellationToken cancellation)
        {
            using FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
            byte[] bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length, cancellation);
            await file.WriteAsync(bytes, 0, bytes.Length, cancellation);
            stream.Close();
        }
    }
}

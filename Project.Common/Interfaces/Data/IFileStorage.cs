using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Interfaces.Data
{
    public interface IFileStorage
    {
        Task<string?> UploadAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream?> DownloadAsync(string fileId);
        Task<bool> DeleteAsync(string fileId);
    }
}

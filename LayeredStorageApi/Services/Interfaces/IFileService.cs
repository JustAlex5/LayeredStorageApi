using Project.Common.Models;

namespace LayeredStorageApi.Services.Interfaces
{
    public interface IFileService
    {
        Task<ResponseModel<int>?> UploadFileAsync(Stream fileStream, string fileName, IFormFile file);
        Task<Stream?> DownloadFileAsync(string fileId);

    }
}

using LayeredStorageApi.Services.Interfaces;
using Project.Common.Interfaces.Data;
using Project.Common.Models;

namespace LayeredStorageApi.Services.Implementations
{
    public class FileService : IFileService
    {

        private readonly IFileStorage _fileStorage;
        private readonly ILogger<FileService> _logger;
        private readonly IIncertBulk _incertBulk;

        public FileService(IFileStorage fileStorage, ILogger<FileService> logger, IIncertBulk incertBulk)
        {
            _fileStorage = fileStorage;
            _logger = logger;
            _incertBulk = incertBulk;
        }

        public async Task<ResponseModel<int>?> UploadFileAsync(Stream fileStream, string fileName , IFormFile file)
        {
            try
            {
                var fileId = await _fileStorage.UploadAsync(fileStream, fileName,file.ContentType);
                _logger.LogInformation("Uploaded file {FileName} with ID {FileId}", fileName, fileId);
                return await _incertBulk.IncertBulkFromBody(fileId,Project.Common.Enums.SourceTypeEnum.FileUpload);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName}", fileName);
                return null;
            }
        }

        // Example method for downloading
        public async Task<Stream?> DownloadFileAsync(string fileId)
        {
            try
            {
                var file = await _fileStorage.DownloadAsync(fileId);
                if (file == null)
                {
                    _logger.LogWarning("File with ID {FileId} not found", fileId);
                }
                return file;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download file with ID {FileId}", fileId);
                return null;
            }
        }
    }
}


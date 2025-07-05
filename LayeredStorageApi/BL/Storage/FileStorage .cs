using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Project.Common.Interfaces.Data;
using Project.Common.Utils.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LayeredStorageApi.BL.Storage
{
    public class FileStorage : IDataStorage
    {
        private readonly string _directoryPath;
        private readonly ILogger<FileStorage> _logger;
        private const string Action = nameof(FileStorage);

        public FileStorage(IConfiguration config, ILogger<FileStorage> logger)
        {
            _directoryPath = config["FileService:BasePath"] ?? "Data";
            _logger = logger;
        }

        private string FileNameHelper(int id) => Path.Combine(_directoryPath, $"IncertBulk_{id}.json");

        public async Task DeleteDataAsync(int id)
        {
            var file = FileNameHelper(id);
            try
            {
                await FileUtils.DeleteFileAsync(_directoryPath, file);
                _logger.LogInformation("{Action}: Deleted file. ID: {Id}", Action, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to delete file. ID: {Id}, File: {File}", Action, id, file);
            }
        }

        public async Task SaveDataAsync(int id, string data)
        {
            var file = FileNameHelper(id);
            try
            {
                await FileUtils.SaveDataAsync(data, _directoryPath, file);
                _logger.LogInformation("{Action}: Saved data to file. ID: {Id}, File: {File}", Action, id, file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to save data to file. ID: {Id}, File: {File}", Action, id, file);
            }
        }

        public async Task<string?> TryGetDataAsync(int id)
        {
            var file = FileNameHelper(id);
            try
            {
                var res = await FileUtils.ReadDataAsync<string>(_directoryPath, file);
                _logger.LogInformation("{Action}: Read data from file. ID: {Id}, File: {File}", Action, id, file);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to read data from file. ID: {Id}, File: {File}", Action, id, file);
                return null;
            }
        }
    }
}

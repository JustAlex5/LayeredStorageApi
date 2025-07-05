using Microsoft.Extensions.Configuration;
using Project.Common.Interfaces.Data;
using Project.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredStorageApi.BL.Storage
{
    public class FileStorage : IDataStorage
    {
        private readonly string _directoryPath;

        public FileStorage(IConfiguration config)
        {
            _directoryPath = config["FileStorage:Path"] ?? "Data";
        }

        private string FileNameHelper(int id) => Path.Combine(_directoryPath, $"Data_{id}.json");

        public async Task DeleteDataAsync(int id)
        {
            await FileUtils.DeleteFileAsync(_directoryPath, FileNameHelper(id));
        }

        public async Task SaveDataAsync(int id, string data)
        {
            await FileUtils.SaveDataAsync(data, _directoryPath, FileNameHelper(id));
        }

        public async Task<string?> TryGetDataAsync(int id)
        {
            return await FileUtils.ReadDataAsync<string>(_directoryPath, FileNameHelper(id));
        }
    }
}

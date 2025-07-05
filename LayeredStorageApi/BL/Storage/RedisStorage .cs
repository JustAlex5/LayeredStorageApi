using Microsoft.Extensions.Options;
using Project.Common.Interfaces.Data;
using Project.Common.Interfaces.Services;
using Project.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredStorageApi.BL.Storage
{
    public class RedisStorage : IDataStorage
    {
        private readonly ICache _cache;
        private readonly CacheConfig _cacheConfig;
        public RedisStorage(ICache cache, IOptions<CacheConfig> options)
        {
            _cacheConfig = options.Value;
            _cache = cache;
        }
        private string CacheKeyHelper(int id) => $"Data/{id}";

        public async Task DeleteDataAsync(int id)
        {
            await _cache.RemoveAsync(CacheKeyHelper(id));
        }

        public async Task SaveDataAsync(int id, string data)
        {
            await _cache.SetRecordAsync(CacheKeyHelper(id), data, _cacheConfig.ExpirationTime);
        }

        public async Task<string?> TryGetDataAsync(int id)
        {
            return await _cache.GetRecordAsync(CacheKeyHelper(id));
        }
    }
}

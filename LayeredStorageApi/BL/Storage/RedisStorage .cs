using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Project.Common.Interfaces.Data;
using Project.Common.Interfaces.Services;
using Project.Common.Models;
using Project.Common.Models.Core;
using System;
using System.Threading.Tasks;

namespace LayeredStorageApi.BL.Storage
{
    public class RedisStorage : IDataStorage
    {
        private readonly ICache _cache;
        private readonly CacheConfig _cacheConfig;
        private readonly ILogger<RedisStorage> _logger;
        private const string Action = nameof(RedisStorage);

        public RedisStorage(ICache cache, IOptions<CacheConfig> options, ILogger<RedisStorage> logger)
        {
            _cacheConfig = options.Value;
            _cache = cache;
            _logger = logger;
        }

        private string CacheKeyHelper(int id) => $"IncertBulk/{id}";

        public async Task DeleteDataAsync(int id)
        {
            var key = CacheKeyHelper(id);
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogInformation("{Action}: Deleted key from Redis. ID: {Id}", Action, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to delete Redis key. ID: {Id}", Action, id);
            }
        }

        public async Task SaveDataAsync(int id, string data)
        {
            var key = CacheKeyHelper(id);
            try
            {
                await _cache.SetRecordAsync(key, data, _cacheConfig.ExpirationTime);
                _logger.LogInformation("{Action}: Saved data to Redis. ID: {Id}", Action, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to save data to Redis. ID: {Id}", Action, id);
            }
        }

        public async Task<string?> TryGetDataAsync(int id)
        {
            var key = CacheKeyHelper(id);
            try
            {
                var res = await _cache.GetRecordAsync(key);
                if (res != null)
                    _logger.LogInformation("{Action}: Data found in Redis. ID: {Id}", Action, id);
                else
                    _logger.LogInformation("{Action}: No data in Redis. ID: {Id}", Action, id);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to get data from Redis. ID: {Id}", Action, id);
                return null;
            }
        }
    }
}

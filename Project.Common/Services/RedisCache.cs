using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Project.Common.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Services
{

    public class RedisCache : ICache
    {
        private readonly ILogger<RedisCache> _logger;
        private readonly IConnectionMultiplexer _connecection;
        private readonly CacheConfig _cacheConfig;
        private readonly string[] _connectionString;

        public RedisCache(ILogger<RedisCache> logger, IConnectionMultiplexer connection, IOptions<CacheConfig> config)
        {
            _logger = logger;
            _connecection = connection;
            _cacheConfig = config.Value;
            _connectionString = _cacheConfig.ConnectionString.Split(",");
        }

        private IDatabase RedisDb => _connecection.GetDatabase();
        private IServer Redisserver => _connecection.GetServer(_connectionString.First());


        public T? GetRecord<T>(string key)
        {
            var res = RedisDb.StringGet(key);
            if (res.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(res.ToString(), _jsonSettings);
            }
            return default(T);
        }

        public async Task<T?> GetRecordAsync<T>(string key)
        {
            var res = await RedisDb.StringGetAsync(key).ConfigureAwait(false);
            if (res.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(res.ToString(),_jsonSettings);
            }
            return default(T);
        }

        public async Task<string?> GetRecordAsync(string key)
        {
            var res = await RedisDb.StringGetAsync(key).ConfigureAwait(false);
            return res;

        }

        public async Task SetRecordAsync<T>(string key, T data, TimeSpan? ttl = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data,_jsonSettings);
                await RedisDb.StringSetAsync(key, json, ttl).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to set cache record for key: {key}");
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await RedisDb.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await RedisDb.KeyExistsAsync(key);
        }

        private static readonly JsonSerializerSettings _jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
    }
}

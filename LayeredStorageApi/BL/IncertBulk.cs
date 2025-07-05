using AutoMapper;
using LayeredStorageApi.DbData;
using Microsoft.Extensions.Options;
using Project.Common.Enums;
using Project.Common.Models;
using Project.Common.Services;
using Project.Common.Utils;
using System;

namespace LayeredStorageApi.BL
{
    public class IncertBulk: IIncertBulk
    {
        private readonly ILogger<IncertBulk> _logger;
        private readonly object _lock = new object();
        private readonly DataLayerContext _context;
        private readonly ICache _redisCache;
        private readonly CacheConfig _cacheConfig;
        private const string Action = nameof(IncertBulk);

        public IncertBulk(ILogger<IncertBulk> logger,DataLayerContext context,ICache cache, IOptions<CacheConfig> options)
        {
            _context = context;
            _logger = logger;
            _redisCache = cache;
            _cacheConfig = options.Value;
        }

        public async Task<ResponseModel<int>>IncertBulkFromBody(string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data) || data == "\"\"")
                    return ResponseFactory.Error<int>("Input data is empty.");
                var entity = new DataStore()
                {
                    Data = data,
                    SourceType = SourceTypeEnum.Manual


                };


                _context.DataStores.Add(entity);
                await _context.SaveChangesAsync();
                var key = CacheKeyHelper(entity.Id);
                var fileName = $"{Action}_{entity.Id}.json";
                await _redisCache.SetRecordAsync(key, data, _cacheConfig.ExpirationTime);
                await WriteToFile.SaveDataAsync(data, "Data", fileName);

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Action} Failed to insert bulk data- {Error}",nameof(IncertBulk),ex.Message); 
                return ResponseFactory.Error<int>(ex.Message);
            }
                
            
        }

        private string CacheKeyHelper(int id)
        {
            return $"{Action}/{id}";
        }

       

    }
}

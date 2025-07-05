using AutoMapper;
using LayeredStorageApi.DbData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Project.Common.Enums;
using Project.Common.Models;
using Project.Common.Services;
using Project.Common.Utils;
using System;
using System.Text.Json;

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
                var fileName = GetFileNameHelper(entity.Id);
                await _redisCache.SetRecordAsync(key, data, _cacheConfig.ExpirationTime);
                await FileUtils.SaveDataAsync(data, "Data", fileName);

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Action} Failed to insert bulk data- {Error}",nameof(IncertBulk),ex.Message); 
                return ResponseFactory.Error<int>(ex.Message);
            }
                
            
        }

        public async Task<ResponseModel<string> >GetDataByIdAsync(int id)
        {
            var cacheKey = CacheKeyHelper(id);
            var fileName = GetFileNameHelper(id);

            try
            {
                var dataFromChache = await _redisCache.GetRecordAsync(cacheKey);
                if (dataFromChache != null) return ResponseFactory.Success(dataFromChache);

                var dataFromFile = await FileUtils.ReadDataAsync<string>("Data", fileName);
                if (dataFromFile != null) return ResponseFactory.Success(dataFromFile);

                var dataFromDb = await _context.DataStores.FirstOrDefaultAsync(x => x.Id == id);

                if (dataFromDb != null) return ResponseFactory.Success(dataFromDb.Data);
                


                return ResponseFactory.Error<string>($"No data found for id {id}.");


            }
            catch (Exception ex)
            {
                return ResponseFactory.Error<string>(ex.Message);
            }
        }

        public async Task<ActionResult<ResponseModel<bool>>> UpdateDataByIdAsync(int id, string data)
        {
            var cacheKey = CacheKeyHelper(id);
            var fileName = GetFileNameHelper(id);

            try
            {
                var dataFromDb = await _context.DataStores.FirstOrDefaultAsync(x => x.Id == id);

                if (dataFromDb == null)
                    return ResponseFactory.Error<bool>("Data with the given ID not found");

                if (string.Equals(dataFromDb.Data, data, StringComparison.OrdinalIgnoreCase))
                    return ResponseFactory.Error<bool>("Nothing to update");

                await _redisCache.RemoveAsync(cacheKey);
                await FileUtils.DeleteFileAsync("Data", fileName);

                dataFromDb.Data = data;
                _context.DataStores.Update(dataFromDb);
                await _context.SaveChangesAsync();

                return ResponseFactory.Success(true, "Data updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Action}: Failed to update data for ID {id}");
                return ResponseFactory.Error<bool>("Failed to update data");
            }
        }

        private string CacheKeyHelper(int id)
        {
            return $"{Action}/{id}";
        }

        private string GetFileNameHelper(int id)
        {
            return $"{Action}_{id}.json";
        }

       

    }
}

using AutoMapper;
using LayeredStorageApi.DbData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Project.Common.Enums;
using Project.Common.Interfaces.Data;
using Project.Common.Interfaces.Services;
using Project.Common.Models;
using Project.Common.Utils;
using System;
using System.Text.Json;

namespace LayeredStorageApi.BL
{
    public class IncertBulk: IIncertBulk
    {
        private readonly ILogger<IncertBulk> _logger;
        private readonly object _lock = new object();
        private readonly ICache _redisCache;
        private readonly IDataRepository _repository;
        private readonly CacheConfig _cacheConfig;
        private readonly IStorageFactory _storageFactory;
        private const string Action = nameof(IncertBulk);

        public IncertBulk(ILogger<IncertBulk> logger,ICache cache, IOptions<CacheConfig> options,IDataRepository repository,IStorageFactory factory)
        {
            _logger = logger;
            _redisCache = cache;
            _cacheConfig = options.Value;
            _repository = repository;
            _storageFactory = factory;
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

                _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();
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

        public async Task<ResponseModel<string>> GetDataByIdAsync(int id)
        {


            foreach (var storage in _storageFactory.GetRetrievalOrder())
            {
                var data = await storage.TryGetDataAsync(id);
                if (data != null)
                {
                    return ResponseFactory.Success(data);
                }
            }
            return ResponseFactory.Error<string>("Id not found",404); 

        }

        public async Task<ResponseModel<bool>> UpdateDataByIdAsync(int id, string data)
        {
            var cacheKey = CacheKeyHelper(id);
            var fileName = GetFileNameHelper(id);

            try
            {
                var dataFromDb = await _repository.GetByIdAsync(id);

                if (dataFromDb == null)
                    return ResponseFactory.Error<bool>("Data with the given ID not found");

                if (string.Equals(dataFromDb.Data, data, StringComparison.OrdinalIgnoreCase))
                    return ResponseFactory.Error<bool>("Nothing to update");

                await _redisCache.RemoveAsync(cacheKey);
                await FileUtils.DeleteFileAsync("Data", fileName);

                dataFromDb.Data = data;
                _repository.UpdateAsync(dataFromDb);
                await _repository.SaveChangesAsync();

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

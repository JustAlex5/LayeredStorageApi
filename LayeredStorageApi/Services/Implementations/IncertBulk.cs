using AutoMapper;
using LayeredStorageApi.DbData;
using LayeredStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Project.Common.Enums;
using Project.Common.Interfaces.Data;
using Project.Common.Interfaces.Services;
using Project.Common.Models;
using Project.Common.Models.Core;
using Project.Common.Utils.Helpers;
using System;
using System.Text.Json;

namespace LayeredStorageApi.Services.Implementations
{
    public class IncertBulk : IIncertBulk
    {
        private readonly ILogger<IncertBulk> _logger;
        private readonly object _lock = new object();
        private readonly ICache _redisCache;
        private readonly IDataRepository _repository;
        private readonly CacheConfig _cacheConfig;
        private readonly IStorageFactory _storageFactory;
        private const string Action = nameof(IncertBulk);

        public IncertBulk(ILogger<IncertBulk> logger, ICache cache, IOptions<CacheConfig> options, IDataRepository repository, IStorageFactory factory)
        {
            _logger = logger;
            _redisCache = cache;
            _cacheConfig = options.Value;
            _repository = repository;
            _storageFactory = factory;
        }

        public async Task<ResponseModel<int>> IncertBulkFromBody(string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data) || data == "\"\"")
                    return ResponseFactory.Error<int>("Input data is empty.");

                var entity = new DataStore
                {
                    Data = data,
                    SourceType = SourceTypeEnum.Manual
                };

                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                foreach (var storage in _storageFactory.GetRetrievalOrder())
                {
                    try
                    {
                        await storage.SaveDataAsync(entity.Id, data);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "{Action}: Failed to write to {StorageType} for ID {Id}", Action, storage.GetType().Name, entity.Id);
                    }
                }

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to insert bulk data - {Error}", Action, ex.Message);
                return ResponseFactory.Error<int>("Internal error while inserting data.");
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
            return ResponseFactory.Error<string>("Id not found", 404);

        }

        public async Task<ResponseModel<bool>> UpdateDataByIdAsync(int id, string data)
        {
            try
            {
                var dataFromDb = await _repository.GetByIdAsync(id);
                if (dataFromDb == null)
                    return ResponseFactory.Error<bool>("Data with the given ID not found");

                if (string.Equals(dataFromDb.Data, data, StringComparison.OrdinalIgnoreCase))
                    return ResponseFactory.Error<bool>("Nothing to update");

                // Update DB
                dataFromDb.Data = data;
                _repository.UpdateAsync(dataFromDb);
                await _repository.SaveChangesAsync();

                // Clear and re-save to other layers
                foreach (var storage in _storageFactory.GetRetrievalOrder())
                {
                    try
                    {
                        await storage.DeleteDataAsync(id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "{Action}: Failed to update {StorageType} for ID {Id}", Action, storage.GetType().Name, id);
                    }
                }

                return ResponseFactory.Success(true, "Data updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to update data for ID {Id}", Action, id);
                return ResponseFactory.Error<bool>("Failed to update data");
            }
        }





    }
}

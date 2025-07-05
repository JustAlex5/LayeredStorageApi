using Microsoft.Extensions.Logging;
using Project.Common.Enums;
using Project.Common.Interfaces.Data;
using Project.Common.Models;
using System;
using System.Threading.Tasks;

namespace LayeredStorageApi.BL.Storage
{
    public class DbStorage : IDataStorage
    {
        private readonly IDataRepository _repository;
        private readonly ILogger<DbStorage> _logger;
        private const string Action = nameof(DbStorage);

        public DbStorage(IDataRepository repository, ILogger<DbStorage> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task DeleteDataAsync(int id)
        {
            _logger.LogWarning("{Action}: Delete operation is not supported for DB storage. ID: {Id}", Action, id);
            throw new NotImplementedException("Delete is not supported for DB storage.");
        }

        public async Task SaveDataAsync(int id, string data)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                {
                    _logger.LogInformation("{Action}: Creating new record. ID: {Id}", Action, id);
                    entity = new DataStore
                    {
                        Id = id,
                        Data = data,
                        SourceType = SourceTypeEnum.Manual
                    };

                    await _repository.AddAsync(entity);
                }
                else
                {
                    _logger.LogInformation("{Action}: Updating existing record. ID: {Id}", Action, id);
                    entity.Data = data;
                    await _repository.UpdateAsync(entity);
                }

                _logger.LogInformation("{Action}: Save operation completed successfully. ID: {Id}", Action, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to save data. ID: {Id}", Action, id);
                throw;
            }
        }

        public async Task<string?> TryGetDataAsync(int id)
        {
            try
            {
                var res = await _repository.GetByIdAsync(id);
                if (res == null)
                {
                    _logger.LogWarning("{Action}: No data found. ID: {Id}", Action, id);
                    return null;
                }

                _logger.LogInformation("{Action}: Data retrieved successfully. ID: {Id}", Action, id);
                return res.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Failed to retrieve data. ID: {Id}", Action, id);
                throw;
            }
        }
    }
}

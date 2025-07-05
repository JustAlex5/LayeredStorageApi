using Project.Common.Interfaces.Data;
using Project.Common.Interfaces.Services;

namespace LayeredStorageApi.BL.Storage
{
    public class StorageFactory : IStorageFactory
    {
        private readonly RedisStorage _redis;
        private readonly FileStorage _file;
        private readonly DbStorage _db;

        public StorageFactory(RedisStorage redis, FileStorage file, DbStorage db)
        {
            _redis = redis;
            _file = file;
            _db = db;
        }

        public IEnumerable<IDataStorage> GetRetrievalOrder()
        {
            yield return _redis;
            yield return _file;
            yield return _db;
        }
    }
}

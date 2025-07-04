using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Services
{
    public interface ICache
    {
        public T? GetRecord<T>(string key);
        public Task<T?> GetRecordAsync<T>(string key);
        public Task<string?> GetRecordAsync(string key);
        public  Task SetRecordAsync<T>(string key, T data, TimeSpan? ttl = null);
        public Task<bool> RemoveAsync(string key);
        public Task<bool> ExistsAsync(string key);









    }
}

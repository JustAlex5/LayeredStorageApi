using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Interfaces.Data
{
    public interface IDataStorage
    {
        Task<string?> TryGetDataAsync(int id);
        Task SaveDataAsync(int id, string data);
        Task DeleteDataAsync(int id);
    }
}

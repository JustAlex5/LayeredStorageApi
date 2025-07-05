using Project.Common.Models;

namespace Project.Common.Interfaces.Data
{
    public interface IDataRepository
    {
        Task<DataStore?> GetByIdAsync(int id);
        Task AddAsync(DataStore entity);
        Task UpdateAsync(DataStore entity);
        Task SaveChangesAsync();
    }
}

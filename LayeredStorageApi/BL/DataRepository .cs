using LayeredStorageApi.DbData;
using Microsoft.EntityFrameworkCore;
using Project.Common.Interfaces.Data;
using Project.Common.Models;


namespace LayeredStorageApi.BL
{
    public class DataRepository : IDataRepository
    {
        private readonly DataLayerContext _context;


        public DataRepository(DataLayerContext context) => _context = context;

        public async Task<DataStore?> GetByIdAsync(int id) =>
            await _context.DataStores.FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(DataStore entity) => await _context.DataStores.AddAsync(entity);

        public async Task UpdateAsync(DataStore entity) => _context.DataStores.Update(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

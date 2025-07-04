using Microsoft.EntityFrameworkCore;
using Project.Common.Models;

namespace LayeredStorageApi.DbData
{
    public class DataLayerContext : DbContext
    {
        public DataLayerContext(DbContextOptions<DataLayerContext> options) : base(options)
        {

        }

        public DbSet<DataStore> DataStores { get; set; }
    }
}

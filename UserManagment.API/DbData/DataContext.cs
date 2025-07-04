using Microsoft.EntityFrameworkCore;
using UserManagment.API.Models;

namespace UserManagment.API.DbData
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using StorageApi.Models;

namespace StorageApi.Data
{
    public class StorageContext : DbContext
    {
        public StorageContext (DbContextOptions<StorageContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
    }
}

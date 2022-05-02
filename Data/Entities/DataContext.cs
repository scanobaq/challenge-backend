using Microsoft.EntityFrameworkCore;

namespace Challenge.Api.Data.Entities
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        
    }
}
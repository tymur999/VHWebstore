using Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    public class VHacksWebstoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public VHacksWebstoreDbContext(DbContextOptions<VHacksWebstoreDbContext> options)
            : base(options)
        {
        }
    }
}

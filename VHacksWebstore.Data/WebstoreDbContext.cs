using Microsoft.EntityFrameworkCore;
using VHacksWebstore.Core.Domain;

namespace VHacksWebstore.Data
{
    public class WebstoreDbContext : DbContext
    {
        public WebstoreDbContext(DbContextOptions<WebstoreDbContext> options)
        : base(options)
        { }
        public DbSet<Product> Products { get; set; }
    }
}

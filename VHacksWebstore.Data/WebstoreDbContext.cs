using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VHacksWebstore.Core.Domain;

namespace VHacksWebstore.Data
{
    public class WebstoreDbContext : IdentityDbContext<WebstoreUser>
    {
        public WebstoreDbContext(DbContextOptions<WebstoreDbContext> options)
        : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> Orders { get; set; }
        public DbSet<ProductRating> Ratings { get; set; }
    }
}

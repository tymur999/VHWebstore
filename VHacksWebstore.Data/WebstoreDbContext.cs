using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VHacksWebstore.Core.Domain;

namespace VHacksWebstore.Data
{
    public class WebstoreDbContext : IdentityDbContext<IdentityUser>
    {
        public WebstoreDbContext(DbContextOptions<WebstoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOrder> Orders { get; set; }

        public virtual DbSet<ProductRating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder
                .Entity<Product>()
                .Property(p => p.Images)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v));
                builder.Entity<Product>()
                    .Property(p => p.Reviews)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v), 
                        v => JsonConvert.DeserializeObject<List<ProductRating>>(v));
            builder
                .Entity<ProductRating>()
                .Property(p => p.Images)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v));
        }
    }
}

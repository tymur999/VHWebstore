using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VHacksWebstore.Core.Domain;

namespace VHacksWebstore.Testing.Integration
{
    public class TestDbContext : IdentityDbContext<WebstoreUser>
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(i=> new {i.ProviderKey, i.LoginProvider});
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(i => new { i.UserId, i.LoginProvider, i.Name });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(i => i.Id);
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(i => new { i.UserId,i.RoleId });
            
            var successfulLoginUser = new WebstoreUser()
            {
                NormalizedEmail = "SUCCESSFUL@GMAIL.COM",
                Email = "Successful@gmail.com",
                EmailConfirmed = true,
                UserName = "Successful@gmail.com",
                NormalizedUserName = "SUCCESSFUL@GMAIL.COM",
                LockoutEnabled = true,
                TwoFactorEnabled = false
            };
            var twoFactorUser = new WebstoreUser()
            {
                NormalizedEmail = "TWOFACTOR@GMAIL.COM",
                Email = "TwoFactor@gmail.com",
                EmailConfirmed = true,
                UserName = "TwoFactor@gmail.com",
                NormalizedUserName = "TWOFACTOR@GMAIL.COM",
                LockoutEnabled = true,
                TwoFactorEnabled = true
            };
            var lockedOutUser = new WebstoreUser()
            {
                NormalizedEmail = "LOCKEDOUT@GMAIL.COM",
                Email = "LockedOut@gmail.com",
                EmailConfirmed = true,
                UserName = "LockedOut@gmail.com",
                NormalizedUserName = "LOCKEDOUT@GMAIL.COM",
                LockoutEnabled = true,
                TwoFactorEnabled = false,
                LockoutEnd = DateTime.Today.AddHours(1)
            };
            var unConfirmedUser = new WebstoreUser()
            {
                NormalizedEmail = "UNCONFIRMED@GMAIL.COM",
                Email = "Unconfirmed@gmail.com",
                EmailConfirmed = false,
                UserName = "Unconfirmed@gmail.com",
                NormalizedUserName = "UNCONFIRMED@GMAIL.COM",
                LockoutEnabled = true,
                TwoFactorEnabled = false,
            };
            var hasher = new PasswordHasher<WebstoreUser>();
            successfulLoginUser.PasswordHash = hasher.HashPassword(successfulLoginUser,"password");
            twoFactorUser.PasswordHash = hasher.HashPassword(twoFactorUser, "password");
            lockedOutUser.PasswordHash = hasher.HashPassword(lockedOutUser, "password");
            unConfirmedUser.PasswordHash = hasher.HashPassword(unConfirmedUser, "password");
            modelBuilder.Entity<WebstoreUser>()
                .HasData(successfulLoginUser,twoFactorUser,lockedOutUser,unConfirmedUser);
        }
    }
}

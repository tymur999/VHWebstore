using Application.Data;
using Application.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(VHacksWebstore.Areas.Identity.IdentityHostingStartup))]
namespace VHacksWebstore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<VHacksUserDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("VHacksUserDbContextConnection"), b => b.MigrationsAssembly("VHacksWebstore")));

                services.AddDefaultIdentity<WebstoreUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<VHacksUserDbContext>();
                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Tokens.AuthenticatorTokenProvider = "Email";
                }
                );
            });
        }
    }
}
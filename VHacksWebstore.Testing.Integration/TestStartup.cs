using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using VHacksWebstore.Core.Domain;

namespace VHacksWebstore.Testing.Integration
{
    public class TestStartup { 
    public TestStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<TestDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            services.AddDefaultIdentity<WebstoreUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<TestDbContext>()
            .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Tokens.AuthenticatorTokenProvider = "Email";
            }
                );
            services.ConfigureApplicationCookie(options =>
            {
            // Cookie settings
            options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddAuthentication()
                .AddGoogle("google", options =>
                {
                    options.ClientId = "83012533104-ig05i17ijineoi46vl3639cj7n4sobd9.apps.googleusercontent.com";
                    options.ClientSecret = "JkdQFDjeHzyX367lgHDusvqL";
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

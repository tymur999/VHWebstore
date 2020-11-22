using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VHacksWebstore.Core.Domain;

namespace VHacksWebstore.Data
{
    public class WebstoreUserDbContext : IdentityDbContext<WebstoreUser>
    {
        public WebstoreUserDbContext(DbContextOptions<WebstoreUserDbContext> options)
        : base(options)
        { }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Application.Domain
{
    public class WebstoreUser : IdentityUser
    {
        public bool TwoFactorType { get; set; }
    }
}

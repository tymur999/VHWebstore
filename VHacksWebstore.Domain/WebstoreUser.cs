using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Application.Domain
{
    public class WebstoreUser : IdentityUser
    {
        public bool TwoFactorType { get; set; }
    }
}

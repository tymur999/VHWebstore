using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHacksWebstore.Core.Domain;

namespace TymurDev.Components.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<WebstoreUser> _userManager;
        private readonly SignInManager<WebstoreUser> _signInManager;

        public IndexModel(
            UserManager<WebstoreUser> userManager,
            SignInManager<WebstoreUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Email { get; set; }
        public IList<Claim> Claims { get; set; }
        public bool IsTwoFactor { get; set; }
        public string Id { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            Email = await _userManager.GetEmailAsync(user);
            Claims = await _userManager.GetClaimsAsync(user);
            IsTwoFactor = await _userManager.GetTwoFactorEnabledAsync(user);
            Id = await _userManager.GetUserIdAsync(user);
            return Page();
        }
    }
}

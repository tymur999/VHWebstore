using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHacksWebstore.Core.Domain;

namespace App.Components.Areas.Identity.Pages.Manage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SignInManager<WebstoreUser> _signInManager;
        private readonly UserManager<WebstoreUser> _userManager;

        public IndexModel(
            UserManager<WebstoreUser> userManager,
            SignInManager<WebstoreUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public InfoModel Info { get; set; }

        public class InfoModel
        {
            public string Email { get; set; }
            public IList<Claim> Claims { get; set; }
            [Display(Name="Two Factor Enabled:")]
            public bool IsTwoFactor { get; set; }
            [Display(Name="Your Id:")]
            public string Id { get; set; }
        }

        [TempData] public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Info = new InfoModel
            {
                Email = await _userManager.GetEmailAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
                IsTwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Id = await _userManager.GetUserIdAsync(user)
            };
            return Page();
        }
    }
}
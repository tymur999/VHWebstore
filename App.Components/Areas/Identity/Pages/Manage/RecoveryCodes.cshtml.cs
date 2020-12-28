using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace App.Components.Areas.Identity.Pages.Manage
{
    [Authorize]
    public class RecoveryCodesModel : PageModel
    {
        private readonly ILogger<RecoveryCodesModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public RecoveryCodesModel(UserManager<IdentityUser> userManager, ILogger<RecoveryCodesModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData] public string[] RecoveryCodes { get; set; }

        [TempData] public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            if (RecoveryCodes == null || RecoveryCodes.Length == 0) return RedirectToPage("./TwoFactorAuthentication");

            return Page();
        }
    }
}
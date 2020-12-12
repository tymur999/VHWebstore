using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VHacksWebstore.Core.Domain;

namespace TymurDev.Components.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class RecoveryCodesModel : PageModel
    {
        private readonly UserManager<WebstoreUser> _userManager;
        private readonly ILogger<RecoveryCodesModel> _logger;

        public RecoveryCodesModel(UserManager<WebstoreUser> userManager, ILogger<RecoveryCodesModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string[] RecoveryCodes { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            if (RecoveryCodes == null || RecoveryCodes.Length == 0)
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }

            return Page();
        }
    }
}

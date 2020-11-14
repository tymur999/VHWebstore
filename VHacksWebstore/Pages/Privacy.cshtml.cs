using System.Threading.Tasks;
using Application.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace VHacksWebstore.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly SignInManager<WebstoreUser> _SignInManager;

        public PrivacyModel(ILogger<PrivacyModel> logger, SignInManager<WebstoreUser> signInManager)
        {
            _logger = logger;
            _SignInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToPage("Index");
        }
        public void OnGet()
        {
        }
    }
}

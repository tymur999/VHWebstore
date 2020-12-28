using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace App.Components.Areas.Identity.Pages
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null) return RedirectToPage("/Index");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound($"Unable to load user with email '{email}'.");

            // Once you add a real email sender, you should remove this code that lets you confirm the account
            /*
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new {area = "Identity", userId, code, returnUrl},
                Request.Scheme);
            await _sender.SendEmailAsync(Email, "Confirm your email",
                $"Confirm your account by clicking <a href='{HtmlEncoder.Default.Encode(EmailConfirmationUrl)}'>here</a>");
            */
            return Page();
        }
    }
}
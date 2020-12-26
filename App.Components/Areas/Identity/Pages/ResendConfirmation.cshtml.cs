using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using VHacksWebstore.Core.Domain;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace App.Components.Areas.Identity.Pages
{
    public class ResendConfirmationModel : PageModel
    {
        private readonly UserManager<WebstoreUser> _userManager;
        private readonly SignInManager<WebstoreUser> _signInManager;
        private readonly IEmailSender _sender;

        public ResendConfirmationModel(UserManager<WebstoreUser> userManager, 
            SignInManager<WebstoreUser> signInManager,
            IEmailSender sender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sender = sender;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Incorrect username or password");
                return Page();
            }
            var email = Input.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect username or password.");
                return Page();
            }
            
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                StatusMessage = "Confirmation email sent";
                return RedirectToPage("./Login");
            }
            var result = await _userManager.CheckPasswordAsync(user, Input.Password);
            if (result)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/ConfirmEmail",
                    null,
                    new { area = "Identity", userId = user.Id, code, returnUrl },
                    Request.Scheme);

                await _sender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                
                StatusMessage = "Confirmation email sent";
                return RedirectToPage("./Login");
            }

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, true);
            if (loginResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Too many login attempts. Try again later");
                return RedirectToPage("./Login");
            }
            ModelState.AddModelError(string.Empty, "Incorrect username or password.");
            return Page();
        }
    }
}

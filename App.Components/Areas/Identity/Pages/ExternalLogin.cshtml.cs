﻿using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace App.Components.Areas.Identity.Pages
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", "Callback", new {returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl});
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl});
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                    info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut) return RedirectToPage("./Lockout");

            // If the user does not have an account, then ask the user to create an account.
            ReturnUrl = returnUrl;
            ProviderDisplayName = info.ProviderDisplayName;
            return Page();
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl});
            }

            if (ModelState.IsValid)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new IdentityUser {UserName = email, Email = email, EmailConfirmed = true};

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        await _signInManager.SignInAsync(user, false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
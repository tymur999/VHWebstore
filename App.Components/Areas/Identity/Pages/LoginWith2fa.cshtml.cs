﻿using System;
using System.ComponentModel.DataAnnotations;
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
    public class LoginWith2faModel : PageModel
    {
        private readonly ILogger<LoginWith2faModel> _logger;
        private readonly IEmailSender _sender;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginWith2faModel(SignInManager<IdentityUser> signInManager, ILogger<LoginWith2faModel> logger,
            IEmailSender sender, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _sender = sender;
            _userManager = userManager;
        }

        [BindProperty] public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null) throw new InvalidOperationException("Unable to load two-factor authentication user.");

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            ReturnUrl = returnUrl;
            RememberMe = rememberMe;
            await _sender.SendEmailAsync(user.Email, "Two factor authentication", $"Your code is {token}");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            returnUrl ??= Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null) throw new InvalidOperationException("Unable to load two-factor authentication user.");

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result =
                await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe,
                    Input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }

            _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
            ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
            return Page();
        }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }
    }
}
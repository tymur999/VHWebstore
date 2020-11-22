using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using VHacksWebstore.Core.Domain;
using System;

namespace VHacksWebstore.Core.App.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<WebstoreUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<WebstoreUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        public event EventHandler<LoginEventArgs> LoginEvent;

        public LoginModel(SignInManager<WebstoreUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<WebstoreUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _logger = logger;
        }
        public class LoginEventArgs : EventArgs
        {
            public string Result { get; set; }
            public WebstoreUser LoginUser { get; set; }
        }
        public void OnLoginHandler(object sender, LoginEventArgs e) => _logger.LogInformation($"{e.LoginUser.Email} - {e.Result}", e);
        public void OnUserNotFoundHandler(object sender, LoginEventArgs e) { }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.FindByNameAsync(Input.Email);
                if(user == null)
                {
                    LoginEvent += OnUserNotFoundHandler;
                    LoginEvent.Invoke(this, new LoginEventArgs { Result = "User Not Found" });
                    return Page();
                }
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    LoginEvent += OnLoginHandler;
                    LoginEvent.Invoke(this, new LoginEventArgs { Result = "Failed Login Without Confirmed Email", LoginUser = user });
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Link(
                        "~/Account/ConfirmEmail",
                        values: new { area = "Identity", userId = user.Id, code, returnUrl });
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email - Resend",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    StatusMessage = "Error: Account is not confirmed with an email. We've resent the confirmation message to your email.";
                    return Page();
                }
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    LoginEvent += OnLoginHandler;
                    LoginEvent.Invoke(this, new LoginEventArgs { Result = "Login Successful", LoginUser = user });
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    LoginEvent += OnLoginHandler;
                    LoginEvent.Invoke(this, new LoginEventArgs { Result = "Login Requires 2FA", LoginUser = user });
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    LoginEvent += OnLoginHandler;
                    LoginEvent.Invoke(this, new LoginEventArgs { Result = "Is Locked Out", LoginUser = user });
                    return RedirectToPage("./Lockout");
                }
                if (user.PasswordHash == null)
                {
                    StatusMessage = "Error: This account is registered with an external login. Unable to login with local account.";
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

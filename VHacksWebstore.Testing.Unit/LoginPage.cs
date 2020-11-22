using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using VHacksWebstore.Core.App.Pages.Account;
using VHacksWebstore.Core.Domain;
using Xunit;
using static VHacksWebstore.Core.App.Pages.Account.LoginModel;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VHacksWebstore.Testing.Unit
{
    public class LoginPageTests
    {

        public LoginPageTests()
        {
            string userId = new Guid().ToString();
            TestUser = new WebstoreUser
            {
                Id = userId,
                NormalizedEmail = "SUCCESSFUL@GMAIL.COM",
                Email = "Successful@gmail.com",
                EmailConfirmed = true,
                UserName = "Successful@gmail.com",
                NormalizedUserName = "SUCCESSFUL@GMAIL.COM",
            };
        }
        public Mock<IHttpContextAccessor> _ca = new Mock<IHttpContextAccessor>();
        public Mock<IUserClaimsPrincipalFactory<WebstoreUser>> _upf = new Mock<IUserClaimsPrincipalFactory<WebstoreUser>>();
        public WebstoreUser TestUser { get; set; }
        [Fact]
        public async Task LoginPage_SuccessfulLogin()
        {
            var userManager = new Mock<UserManager<WebstoreUser>>(new Mock<IUserStore<WebstoreUser>>().Object,
                null, null, null, null, null, null, null, null);
            userManager.Setup(i => i.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(TestUser);
            var signInManager = new Mock<SignInManager<WebstoreUser>>(userManager.Object, _ca.Object, _upf.Object, null, null, null, null);
            signInManager.Setup(i => i.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);

            var page = new LoginModel(signInManager.Object, new Mock<ILogger<LoginModel>>().Object, userManager.Object, new Mock<IEmailSender>().Object);
            page.Input = new InputModel() { Email = "Successful@gmail.com", Password = "password", RememberMe = true };
            var recievedEvent = await Assert.RaisesAnyAsync<LoginEventArgs>(a => page.LoginEvent += a,
                a => page.LoginEvent -= a,
                async () => await page.OnPostAsync());
            Assert.Equal("Login Successful", recievedEvent.Arguments.Result);
        }
        [Fact]
        public async Task LoginPage_UserNotFound()
        {
            var userManager = new Mock<UserManager<WebstoreUser>>(new Mock<IUserStore<WebstoreUser>>().Object,
                null, null, null, null, null, null, null, null);
            userManager.Setup(i => i.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((WebstoreUser)null);
            var signInManager = new Mock<SignInManager<WebstoreUser>>(userManager.Object, _ca.Object, _upf.Object, null, null, null, null);

            var page = new LoginModel(signInManager.Object, new Mock<ILogger<LoginModel>>().Object, userManager.Object, new Mock<IEmailSender>().Object);
            page.Input = new InputModel() { Email = "UserNotFound@gmail.com", Password = "password", RememberMe = true };

            var recievedEvent = await Assert.RaisesAnyAsync<LoginEventArgs>(a => page.LoginEvent += a,
                a => page.LoginEvent -= a,
                async () => await page.OnPostAsync());
            Assert.Equal("User Not Found", recievedEvent.Arguments.Result);
        }
        [Fact]
        public void LoginPage_IncorrectPassword()
        {
            var userManager = new Mock<UserManager<WebstoreUser>>(new Mock<IUserStore<WebstoreUser>>().Object,
                null, null, null, null, null, null, null, null);
            userManager.Setup(i => i.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(TestUser);
            var signInManager = new Mock<SignInManager<WebstoreUser>>(userManager.Object, _ca.Object, _upf.Object, null, null, null, null);
            signInManager.Setup(i => i.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.NotAllowed);

            var page = new LoginModel(signInManager.Object, new Mock<ILogger<LoginModel>>().Object, userManager.Object, new Mock<IEmailSender>().Object);
            page.Input = new InputModel() { Email = "Successful@gmail.com", Password = "password", RememberMe = true };
            Assert.False(Assert.RaisesAnyAsync<LoginEventArgs>(a => page.LoginEvent += a,
                a => page.LoginEvent -= a,
                async () => await page.OnPostAsync()).IsCompletedSuccessfully);
        }
        [Fact]
        public async Task LoginPage_IsLockedOut()
        {
            var userManager = new Mock<UserManager<WebstoreUser>>(new Mock<IUserStore<WebstoreUser>>().Object,
                null, null, null, null, null, null, null, null);
            userManager.Setup(i => i.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(TestUser);
            var signInManager = new Mock<SignInManager<WebstoreUser>>(userManager.Object, _ca.Object, _upf.Object, null, null, null, null);
            signInManager.Setup(i => i.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.LockedOut);

            var page = new LoginModel(signInManager.Object, new Mock<ILogger<LoginModel>>().Object, userManager.Object, new Mock<IEmailSender>().Object);
            page.Input = new InputModel() { Email = "Successful@gmail.com", Password = "password", RememberMe = true };
            var result = await Assert.RaisesAnyAsync<LoginEventArgs>(a => page.LoginEvent += a,
                a => page.LoginEvent -= a,
                async () => await page.OnPostAsync());
            Assert.Equal("Is Locked Out", result.Arguments.Result);
        }
        [Fact]
        public async Task LoginPage_Is2FA()
        {
            var userManager = new Mock<UserManager<WebstoreUser>>(new Mock<IUserStore<WebstoreUser>>().Object,
                null, null, null, null, null, null, null, null);
            userManager.Setup(i => i.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(TestUser);
            var signInManager = new Mock<SignInManager<WebstoreUser>>(userManager.Object, _ca.Object, _upf.Object, null, null, null, null);
            signInManager.Setup(i => i.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.TwoFactorRequired);

            var page = new LoginModel(signInManager.Object, new Mock<ILogger<LoginModel>>().Object, userManager.Object, new Mock<IEmailSender>().Object);
            page.Input = new InputModel() { Email = "Successful@gmail.com", Password = "password", RememberMe = true };
            var result = await Assert.RaisesAnyAsync<LoginEventArgs>(a => page.LoginEvent += a,
                a => page.LoginEvent -= a,
                async () => await page.OnPostAsync());
            Assert.Equal("Login Requires 2FA", result.Arguments.Result);
        }
        [Fact]
        public async Task LoginPage_EmailNotConfirmed()
        {
            var userManager = new Mock<UserManager<WebstoreUser>>(new Mock<IUserStore<WebstoreUser>>().Object,
                null, null, null, null, null, null, null, null);
            TestUser.EmailConfirmed = false;
            userManager.Setup(i => i.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(TestUser);
            userManager.Setup(i => i.GenerateEmailConfirmationTokenAsync(It.IsAny<WebstoreUser>())).ReturnsAsync("0");
            var signInManager = new Mock<SignInManager<WebstoreUser>>(userManager.Object, _ca.Object, _upf.Object, null, null, null, null);

            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost");
            var page = new LoginModel(signInManager.Object, new Mock<ILogger<LoginModel>>().Object, userManager.Object, new Mock<IEmailSender>().Object) { Url = urlHelper.Object };
            page.Input = new InputModel() { Email = "Successful@gmail.com", Password = "password", RememberMe = true };
            var result = await Assert.RaisesAnyAsync<LoginEventArgs>(a => page.LoginEvent += a,
                a => page.LoginEvent -= a,
                async () => await page.OnPostAsync());
            TestUser.EmailConfirmed = true;
            Assert.Equal("Failed Login Without Confirmed Email", result.Arguments.Result);
        }
    }
}


using System.Collections.Generic;
using System.Threading.Tasks;
using VHacksWebstore.Testing.Integration.Helpers;
using Xunit;
using System.Net;
using System.Net.Http;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace VHacksWebstore.Testing.Integration
{
    public class LoginPageTests : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;

        public LoginPageTests(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions(){
                AllowAutoRedirect = false});
        }
        [Fact]
        public async Task LoginPage_SuccessfulLogin()
        {
            var defaultPage = await _client.GetAsync("/Account/Login");
            defaultPage.EnsureSuccessStatusCode();
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='account']");
            var profileWithUserName = await _client.SendAsync(
                form,
                (IHtmlElement)form.QuerySelector("btn[id='submitLogin']"),
                new Dictionary<string, string> { ["username"] = "Successful@gmail.com", ["password"] = "password" });
            Assert.Equal(HttpStatusCode.Redirect, profileWithUserName.StatusCode);
            Assert.Equal("/", profileWithUserName.Headers.Location.OriginalString);

        }
        
        [Theory]
        [InlineData("bad@gmail.com","Pass1234!")]
        [InlineData("Successful@gmail.com", "pass")]
        public async Task LoginPage_BadLogin(string userName, string pass)
        {
            var defaultPage = await _client.GetAsync("/Account/Login");
            defaultPage.EnsureSuccessStatusCode();
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='account']");
            var profileWithUserName = await _client.SendAsync(
                form,
                (IHtmlElement)form.QuerySelector("btn[id='submitLogin']"),
                new Dictionary<string, string> { ["username"] = userName, ["password"] = pass });
            Assert.NotEqual(HttpStatusCode.InternalServerError, profileWithUserName.StatusCode);
            Assert.NotEqual(HttpStatusCode.Redirect, profileWithUserName.StatusCode);
        }
        [Fact]
        public async Task LoginPage_2FA()
        {
            var defaultPage = await _client.GetAsync("/Account/Login");
            defaultPage.EnsureSuccessStatusCode();
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='account']");
            var profileWithUserName = await _client.SendAsync(
                form,
                (IHtmlElement)form.QuerySelector("btn[id='submitLogin']"),
                new Dictionary<string, string> { ["username"] = "TwoFactor@gmail.com", ["password"] = "password" });
            Assert.Equal(HttpStatusCode.Redirect, profileWithUserName.StatusCode);
            Assert.Equal("/Account/LoginWith2fa?ReturnUrl=~%2F&RememberMe=False", profileWithUserName.Headers.Location.OriginalString);
        }
        [Fact]
        public async Task LoginPage_Unconfirmed()
        {
            var defaultPage = await _client.GetAsync("/Account/Login");
            defaultPage.EnsureSuccessStatusCode();
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='account']");
            var profileWithUserName = await _client.SendAsync(
                form,
                (IHtmlElement)form.QuerySelector("btn[id='submitLogin']"),
                new Dictionary<string, string> { ["username"] = "Unconfirmed@gmail.com", ["password"] = "password" });
            Assert.Equal(HttpStatusCode.Redirect, profileWithUserName.StatusCode);
            Assert.Equal("/Account/ConfirmEmail", profileWithUserName.Headers.Location.OriginalString);
        }
    }
}


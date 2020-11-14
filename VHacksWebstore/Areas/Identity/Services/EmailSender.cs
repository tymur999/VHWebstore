using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace VHacksWebstore.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        private string SendGridUser { get; set; }
        private string SendGridKey { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
            SendGridUser = Configuration["SendGridUser"];
            SendGridKey = Configuration["SendGridKey"];
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("tymur.arsent@gmail.com", SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}

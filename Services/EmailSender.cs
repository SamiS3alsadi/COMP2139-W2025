using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace COMP2139_ICE.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _enableSsl;
        private readonly string _userName;
        private readonly string _password;

        public EmailSender(IConfiguration configuration)
        {
            // Read SMTP settings from configuration
            _host = configuration["SmtpSettings:Host"];
            _port = int.Parse(configuration["SmtpSettings:Port"]);
            _enableSsl = bool.Parse(configuration["SmtpSettings:EnableSsl"]);
            _userName = configuration["SmtpSettings:UserName"];
            _password = configuration["SmtpSettings:Password"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Create email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("COMP2139 App", _userName));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            message.Body = bodyBuilder.ToMessageBody();

            // Send email using SMTP
            using var client = new SmtpClient();
            await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_userName, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
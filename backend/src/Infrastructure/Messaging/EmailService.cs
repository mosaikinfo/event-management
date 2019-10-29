using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Messaging
{
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendMailAsync(MailSettings mailSettings, EmailMessage mail)
        {
            var message = new MimeMessage();
            message.From.AddRange(mail.From.Select(a => new MailboxAddress(a)));
            message.To.AddRange(mail.To.Select(a => new MailboxAddress(a)));
            message.Subject = mail.Subject;
            message.Body = new TextPart("plain") { Text = mail.Body };

            using (var client = new SmtpClient())
            {
                _logger.LogInformation("Connecting to server.");

                await client.ConnectAsync(
                    mailSettings.SmtpHost,
                    mailSettings.SmtpPort,
                    SecureSocketOptions.Auto);

                if (!string.IsNullOrEmpty(mailSettings.SmtpUsername))
                {
                    _logger.LogInformation(
                        "Authenticate with user {user}", mailSettings.SmtpUsername);

                    await client.AuthenticateAsync(
                        mailSettings.SmtpUsername, 
                        mailSettings.SmtpPassword);
                }

                _logger.LogInformation("Sending mail message.");
                await client.SendAsync(message);
                _logger.LogInformation("Disconnecting from server.");
                await client.DisconnectAsync(true);
            }
        }
    }
}

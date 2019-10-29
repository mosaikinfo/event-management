using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Messaging
{
    public class EmailService : IEmailService
    {
        public async Task SendMailAsync(MailSettings mailSettings, EmailMessage mail)
        {
            var message = new MimeMessage();
            message.From.AddRange(mail.From.Select(a => new MailboxAddress(a)));
            message.To.AddRange(mail.To.Select(a => new MailboxAddress(a)));
            message.Subject = mail.Subject;
            message.Body = new TextPart("plain") { Text = mail.Body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    mailSettings.SmtpHost,
                    mailSettings.SmtpPort,
                    mailSettings.UseStartTls);

                if (mailSettings.UseStartTls)
                {
                    await client.AuthenticateAsync(
                        mailSettings.SmtpUsername, 
                        mailSettings.SmtpPassword);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}

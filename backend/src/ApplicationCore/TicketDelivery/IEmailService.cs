using EventManagement.ApplicationCore.Models;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    /// <summary>
    /// The service to send e-mail messages via SMTP.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send an e-mail.
        /// </summary>
        /// <param name="mailSettings">SMTP settings how to transfer the e-mail.</param>
        /// <param name="mail">The e-mail message.</param>
        Task SendMailAsync(MailSettings mailSettings, EmailMessage mail);
    }
}
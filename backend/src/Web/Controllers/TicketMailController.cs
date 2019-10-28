using EventManagement.Infrastructure.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to send tickets via mail.
    /// </summary>
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class TicketMailController : ControllerBase
    {
        private readonly EventsDbContext _context;

        public TicketMailController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Send a ticket via e-mail.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        [HttpPost("tickets/{ticketId}/mail")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> SendMailAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(e => e.Event)
                .ThenInclude(e => e.MailSettings)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                return NotFound(new ProblemDetails
                { Detail = "Ticket with id not found." });

            var settings = ticket.Event.MailSettings;

            // TODO: Validate mail settings.

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings.SenderAddress));
            message.To.Add(new MailboxAddress(ticket.Mail));
            message.Subject = settings.Subject;
            message.Body = new TextPart("plain")
            {
                Text = settings.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(settings.SmtpHost, settings.SmtpPort, settings.UseStartTls);

                if (settings.UseStartTls)
                {
                    await client.AuthenticateAsync(settings.SmtpUsername, settings.SmtpPassword);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return Ok();
        }
    }
}
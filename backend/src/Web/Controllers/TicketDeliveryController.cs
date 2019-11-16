using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketDelivery;
using EventManagement.ApplicationCore.Tickets;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Models;
using EventManagement.WebApp.Shared.Mvc;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// API to deliver tickets to the buyer by various ways (e-mail, sms, etc).
    /// </summary>
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class TicketDeliveryController : EventManagementController
    {
        private readonly EventsDbContext _context;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ITicketDeliveryService _ticketDeliveryService;
        private readonly ITicketRedirectService _ticketRedirectService;
        private readonly IBackgroundJobClient _backgroundJobs;

        public TicketDeliveryController(EventsDbContext context,
                                        ITicketsRepository ticketsRepository,
                                        ITicketDeliveryService ticketDeliveryService,
                                        ITicketRedirectService ticketRedirectService,
                                        IBackgroundJobClient backgroundJobs)
        {
            _context = context;
            _ticketsRepository = ticketsRepository;
            _ticketDeliveryService = ticketDeliveryService;
            _ticketRedirectService = ticketRedirectService;
            _backgroundJobs = backgroundJobs;
        }

        /// <summary>
        /// Send a ticket via e-mail.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        [HttpPost("tickets/{ticketId}/sendMail")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> SendMailAsync(Guid ticketId)
        {
            var ticket = await _ticketsRepository.GetAsync(ticketId);
            if (ticket == null)
                return NotFound(new ProblemDetails
                { Detail = "Ticket with id not found." });

            await SendMailAsync(ticket);
            return Ok();
        }

        /// <summary>
        /// Send multiple tickets via e-mail at once.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="spec">Specifies the list of tickets you want to send.</param>
        /// <returns>result of the batch send.</returns>
        [HttpPost("events/{eventId}/tickets/sendMails")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<BatchSendResult> SendBatchMailsAsync(Guid eventId, TicketsSendSpecification spec)
        {
            var ticketTypes = spec.TicketTypes ?? new List<Guid>();

            var query = _context.Tickets
                .AsNoTracking()
                .Where(x => x.EventId == eventId)
                .Where(x => ticketTypes.Contains(x.TicketTypeId));

            if (!spec.SendAll)
            {
                query = query.Where(x => !x.IsDelivered);
            }

            var tickets = await query.ToListAsync();
            var ticketsWithEmail = tickets.Where(x => x.Mail != null).ToList();
            var ticketsWithoutEmail = tickets.Where(x => x.Mail == null).ToList();

            if (!spec.DryRun)
            {
                foreach (ApplicationCore.Models.Ticket ticket in ticketsWithEmail)
                {
                    await SendMailAsync(ticket);
                }
            }

            return new BatchSendResult
            {
                DryRun = spec.DryRun,
                MailsSent = ticketsWithEmail.Count,
                TicketsWithoutEmailAddress = ticketsWithoutEmail.Count
            };
        }

        private async Task SendMailAsync(ApplicationCore.Models.Ticket ticket)
        {
            var deliveryType = TicketDeliveryType.Email;
            await _ticketDeliveryService.ValidateAsync(ticket.Id, deliveryType);

            string validationUriFormat = GetTicketValidationUriFormatString();
            string validationUri = GetTicketValidationUri(ticket.TicketSecret);
            string homepageUrl = await _ticketRedirectService.GetRedirectUrlAsync(ticket.Id, validationUri);

            _backgroundJobs.Enqueue(() => _ticketDeliveryService
                .SendTicketAsync(ticket.Id, deliveryType, validationUriFormat, homepageUrl));
        }
    }
}
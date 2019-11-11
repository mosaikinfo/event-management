using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketDelivery;
using EventManagement.ApplicationCore.Tickets;
using EventManagement.WebApp.Shared.Mvc;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ITicketDeliveryService _ticketDeliveryService;
        private readonly ITicketRedirectService _ticketRedirectService;
        private readonly IBackgroundJobClient _backgroundJobs;

        public TicketDeliveryController(ITicketsRepository ticketsRepository,
                                        ITicketDeliveryService ticketDeliveryService,
                                        ITicketRedirectService ticketRedirectService,
                                        IBackgroundJobClient backgroundJobs)
        {
            _ticketsRepository = ticketsRepository;
            _ticketDeliveryService = ticketDeliveryService;
            _ticketRedirectService = ticketRedirectService;
            _backgroundJobs = backgroundJobs;
        }

        /// <summary>
        /// Send a ticket via e-mail.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        [HttpPost("tickets/{ticketId}/mail")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> SendMailAsync(Guid ticketId)
        {
            var ticket = await _ticketsRepository.GetAsync(ticketId);
            if (ticket == null)
                return NotFound(new ProblemDetails
                { Detail = "Ticket with id not found." });

            var deliveryType = TicketDeliveryType.Email;
            await _ticketDeliveryService.ValidateAsync(ticketId, deliveryType);

            string validationUriFormat = GetTicketValidationUriFormatString();
            string validationUri = GetTicketValidationUri(ticket.TicketSecret);
            string homepageUrl = await _ticketRedirectService.GetRedirectUrlAsync(ticketId, validationUri);

            _backgroundJobs.Enqueue(() => _ticketDeliveryService
                .SendTicketAsync(ticketId, deliveryType, validationUriFormat, homepageUrl));

            return Ok();
        }
    }
}
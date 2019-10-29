using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
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
    public class TicketDeliveryController : ControllerBase
    {
        private readonly ITicketDeliveryService _ticketDeliveryService;

        public TicketDeliveryController(ITicketDeliveryService ticketDeliveryService)
        {
            _ticketDeliveryService = ticketDeliveryService;
        }

        /// <summary>
        /// Send a ticket via e-mail.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        [HttpPost("tickets/{ticketId}/mail")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> SendMailAsync(Guid ticketId)
        {
            try
            {
                await _ticketDeliveryService.SendTicketAsync(ticketId, TicketDeliveryType.Email);
            }
            catch (TicketNotFoundException)
            {
                return NotFound(new ProblemDetails 
                    { Detail = "Ticket with id not found." });
            }
            return Ok();
        }
    }
}
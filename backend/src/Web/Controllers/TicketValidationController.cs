using AutoMapper;
using EventManagement.ApplicationCore.Tickets;
using EventManagement.Identity;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// API for the ticket validation (check-in) process at the entrance of the event.
    /// 
    /// Tickets can be validated by scanning the QR code that is printed on the ticket
    /// or by entering the ticket number manually.
    /// 
    /// If the event is a conference, a dialog will with some questions will be displayed.
    /// </summary>
    [OpenApiIgnore]
    [AllowAnonymous]
    public class TicketValidationController : EventManagementController
    {
        private readonly EventsDbContext _context;
        private readonly ITicketRedirectService _ticketRedirectService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TicketValidationController(EventsDbContext context,
                                          ITicketRedirectService ticketRedirectService,
                                          IMapper mapper,
                                          ILogger<TicketValidationController> logger)
        {
            _context = context;
            _ticketRedirectService = ticketRedirectService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Validate a ticket manually by entering the ticket number.
        /// You must be authorized to use the admin backend to use this method.
        /// </summary>
        /// <param name="number">Ticket number that is shown on the ticket.</param>
        /// <returns>result page that tells whether the ticket was valid or not.</returns>
        [Authorize(EventManagementConstants.AdminApi.PolicyName)]
        [HttpGet, HttpPost]
        [Route("tickets/validate")]
        public async Task<IActionResult> ValidateTicketByNumberAsync(string number)
        {
            if (number == null)
                return NotFound();

            ApplicationCore.Models.Ticket ticket =
                await FindTicketAsync(e => e.TicketNumber == number);

            return await ValidateTicketAsync(ticket, ticketNumber: number);
        }

        /// <summary>
        /// Validate a ticket by the URL that was contained in the QR Code.
        /// </summary>
        /// <param name="secret">The secret key that was stored within the URL.</param>
        /// <returns>result page that tells whether the ticket was valid or not.</returns>
        [HttpGet("v/{secret}")]
        public async Task<IActionResult> ValidateTicketByQrCodeValueAsync(string secret)
        {
            if (secret == null)
                return NotFound();

            ApplicationCore.Models.Ticket ticket =
                await FindTicketAsync(e => e.TicketSecret == secret);

            return await ValidateTicketAsync(ticket, ticketSecret: secret);
        }

        private async Task<IActionResult> ValidateTicketAsync(ApplicationCore.Models.Ticket ticket,
                                                              string ticketNumber = null,
                                                              string ticketSecret = null)
        {
            if (ticket == null)
            {
                string lookupValueType = ticketNumber == null ? "secret" : "number";
                string lookupValue = ticketNumber ?? ticketSecret;
                _logger.LogInformation(
                    $"Ticket with {lookupValueType} {lookupValue} was not found in the database.", lookupValue);
                return TicketNotFound();
            }

            ClaimsPrincipal currentUser = await TryGetAuthenticatedUser();

            // TODO: check if the user has the permission for the event.

            if (!currentUser.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Unauthorized. Redirect to event homepage.");

                string validationUri = GetTicketValidationUri(ticket.TicketSecret);
                string redirectUrl = await _ticketRedirectService.GetRedirectUrlAsync(ticket.Id, validationUri);

                return Redirect(redirectUrl);
            }

            var currentEvent = ticket.Event;
            if (currentEvent == null)
            {
                // try to get the event for which the master qr code was issued.
                UserContext userContext = User.GetContext();
                if (userContext?.EventId != null)
                {
                    currentEvent = _context.Events.Find(userContext.EventId);
                }
            }
            if (currentEvent != null && currentEvent.IsConference)
            {
                return ConferenceCheckInDialog(ticket);
            }

            if (ticket.Validated)
            {
                _logger.LogInformation("The ticket has already been used before.");
                return View("TicketUsed", ticket);
            }
            ticket.Validated = true;
            _context.SaveChanges();
            return View("TicketValid", ticket);
        }

        private Task<ApplicationCore.Models.Ticket> FindTicketAsync(
            Expression<Func<ApplicationCore.Models.Ticket, bool>> filter)
        {
            return _context.Tickets
                    .Include(e => e.Event)
                    .Include(e => e.TicketType)
                    .SingleOrDefaultAsync(filter);
        }

        private IActionResult TicketNotFound()
        {
            ViewBag.ErrorMessage = "Dieses Ticket existiert leider nicht!";
            return View("TicketError");
        }

        private IActionResult ConferenceCheckInDialog(ApplicationCore.Models.Ticket ticket)
        {
            var model = _mapper.Map<Models.ConferenceDialogModel>(ticket);
            return View("ConferenceDialog", model);
        }
    }
}
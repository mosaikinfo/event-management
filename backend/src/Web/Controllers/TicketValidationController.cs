using EventManagement.ApplicationCore.Tickets;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Shared.Mvc;
using Microsoft.AspNetCore.Authentication;
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
    /// Controller to validate tickets either by scanning the QR Code
    /// or by entering the ticket number manually.
    /// </summary>
    [OpenApiIgnore]
    [AllowAnonymous]
    public class TicketValidationController : EventManagementController
    {
        private readonly EventsDbContext _context;
        private readonly ITicketRedirectService _ticketRedirectService;
        private readonly ILogger _logger;

        public TicketValidationController(EventsDbContext context,
                                          ITicketRedirectService ticketRedirectService,
                                          ILogger<TicketValidationController> logger)
        {
            _context = context;
            _ticketRedirectService = ticketRedirectService;
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

            if (ticket == null)
            {
                _logger.LogInformation(
                    "Ticket with number {number} was not found in the database.",
                    number);
                return TicketNotFound();
            }
            return await ValidateTicketAsync(ticket);
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

            if (ticket == null)
            {
                _logger.LogInformation("Ticket with secret {secret} was not found in the database.", secret);
                return TicketNotFound();
            }
            return await ValidateTicketAsync(ticket);
        }

        private async Task<IActionResult> ValidateTicketAsync(ApplicationCore.Models.Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            ClaimsPrincipal currentUser = await TryGetAuthenticatedUser();

            // TODO: check if the user has the permission for the event.

            if (!currentUser.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Unauthorized. Redirect to event homepage.");

                string validationUri = GetTicketValidationUri(ticket.TicketSecret);
                string redirectUrl = await _ticketRedirectService.GetRedirectUrlAsync(ticket.Id, validationUri);

                return Redirect(redirectUrl);
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

        private async Task<ClaimsPrincipal> TryGetAuthenticatedUser()
        {
            // check default auth cookie.
            if (!User.Identity.IsAuthenticated)
            {
                var auth = await HttpContext.AuthenticateAsync(
                    EventManagementConstants.MasterQrCode.AuthenticationScheme);
                // check master qr auth cookie.
                if (auth.Succeeded)
                {
                    return auth.Principal;
                }
            }
            return User;
        }

        private IActionResult TicketNotFound()
        {
            ViewBag.ErrorMessage = "Dieses Ticket existiert leider nicht!";
            return View("TicketError");
        }
    }
}
using EventManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [OpenApiIgnore]
    [AllowAnonymous]
    public class TicketValidationController : Controller
    {
        private readonly EventsDbContext _context;
        private readonly ILogger _logger;

        public TicketValidationController(EventsDbContext context,
                                          ILogger<TicketValidationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("v/{secret}")]
        public async Task<IActionResult> ValidateTicketAsync(string secret)
        {
            ClaimsPrincipal currentUser = await TryGetAuthenticatedUser();

            ApplicationCore.Models.Ticket ticket =
                _context.Tickets
                    .Include(e => e.Event)
                    .Include(e => e.TicketType)
                    .SingleOrDefault(e => e.TicketSecret == secret);

            if (ticket == null)
            {
                _logger.LogInformation("Ticket with secret {id} was not found in the database.", secret);
                return TicketNotFound();
            }

            // TODO: check if the user has the permission for the event.

            if (!currentUser.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Unauthorized. Redirect to event homepage.");
                return Redirect(ticket.Event.HomepageUrl);
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
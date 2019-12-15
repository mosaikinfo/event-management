using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Shared.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// RPC-style API for the check-in dialog that is used for conferences.
    /// </summary>
    [ApiController]
    [OpenApiIgnore]
    public class CheckInDialogController : EventManagementController
    {
        private readonly EventsDbContext _context;
        private readonly ILogger _logger;

        public CheckInDialogController(EventsDbContext context,
                                          ILogger<CheckInDialogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("checkin/save.dialogResult")]
        public async Task<IActionResult> SaveDialogResultAsync(Models.ConferenceDialogResult model)
        {
            ClaimsPrincipal currentUser = await TryGetAuthenticatedUser();
            // TODO: check if the user has the permission for the event.
            if (!currentUser.Identity.IsAuthenticated)
                return Unauthorized();

            var ticket = await _context.Tickets.FindAsync(model.TicketId);

            if (ticket == null)
            {
                ModelState.AddModelError(
                    nameof(model.TicketId), "Ticket not found in database.");
                return BadRequest();
            }

            return Ok();
        }
    }
}
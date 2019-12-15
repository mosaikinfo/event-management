using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Shared.Mvc;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
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
        private readonly EventsDbContext _db;

        public CheckInDialogController(EventsDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpPost("checkin/setTermsAccepted")]
        public async Task<IActionResult> SetTermsAcceptedAsync(ConferenceDialogResult model)
        {
            var context = await ValidateAndReturnContextAsync(model);
            if (context.Result != null)
                return context.Result;
            // TODO: log audit event.
            context.Ticket.TermsAccepted = true;
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost("checkin/complete")]
        public async Task<IActionResult> CompleteCheckInAsync(ConferenceDialogResult model)
        {
            var context = await ValidateAndReturnContextAsync(model);
            if (context.Result != null)
                return context.Result;
            // TODO: log audit event.
            context.Ticket.Validated = true;
            _db.SaveChanges();
            return Ok();
        }

        private async Task<DialogContext> ValidateAndReturnContextAsync(ConferenceDialogResult model)
        {
            var context = new DialogContext
            {
                User = await TryGetAuthenticatedUser()
            };
            // TODO: check if the user has the permission for the event.
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Result = Unauthorized();
                return context;
            }
            context.Ticket = await _db.Tickets.FindAsync(model.TicketId);
            if (context.Ticket == null)
            {
                ModelState.AddModelError(
                    nameof(model.TicketId), "Ticket not found in database.");
                context.Result = BadRequest();
                return context;
            }
            return context;
        }

        private class DialogContext
        {
            public ClaimsPrincipal User { get; set; }
            public ApplicationCore.Models.Ticket Ticket { get; set; }
            public IActionResult Result { get; set; }
        }
    }

    public class ConferenceDialogResult
    {
        [Required]
        public Guid TicketId { get; set; }
    }
}
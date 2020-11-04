using EventManagement.ApplicationCore.Auditing;
using EventManagement.ApplicationCore.TicketSupport;
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
        private readonly IAuditEventLog _auditEventLog;
        private readonly ISupportTicketRepository _supportTickets;

        public CheckInDialogController(EventsDbContext dbContext,
                                       IAuditEventLog auditEventLog,
                                       ISupportTicketRepository supportTickets)
        {
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _auditEventLog = auditEventLog ?? throw new ArgumentNullException(nameof(auditEventLog));
            _supportTickets = supportTickets ?? throw new ArgumentNullException(nameof(supportTickets));
        }

        [HttpPost("checkin/setTermsAccepted")]
        public async Task<IActionResult> SetTermsAcceptedAsync(ConferenceDialogResult model)
        {
            var context = await ValidateAndReturnContextAsync(model);
            if (context.Result != null)
                return context.Result;

            await _auditEventLog.AddAsync(new ApplicationCore.Models.AuditEvent
            {
                Time = DateTime.UtcNow,
                TicketId = model.TicketId,
                Action = EventManagementConstants.Auditing.Actions.TermsAccepted,
                Detail = "Die Einverständniserklärung der Eltern wurde beim Check-in abgegeben."
            });

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

            await _auditEventLog.AddAsync(new ApplicationCore.Models.AuditEvent
            {
                Time = DateTime.UtcNow,
                TicketId = model.TicketId,
                Action = EventManagementConstants.Auditing.Actions.TicketValidated,
                Detail = $"Check-in war erfolgreich."
            });

            context.Ticket.Validated = true;
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost("checkin/failed")]
        public async Task<IActionResult> CheckInFailedAsync(CheckInFailedResult model)
        {
            var context = await ValidateAndReturnContextAsync(model);
            if (context.Result != null)
                return context.Result;

            await _auditEventLog.AddAsync(new ApplicationCore.Models.AuditEvent
            {
                Time = DateTime.UtcNow,
                TicketId = model.TicketId,
                Action = EventManagementConstants.Auditing.Actions.TicketValidated,
                Level = ApplicationCore.Models.AuditEventLevel.Fail,
                Detail = $"Check-in fehlgeschlagen. Grund: {model.Reason}."
            });

            var supportTicket = await _supportTickets
                .CreateSupportTicketAsync(model.TicketId, model.Reason);

            return Ok(new { supportTicket.SupportNumber });
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

    public class CheckInFailedResult : ConferenceDialogResult
    {
        [Required]
        public string Reason { get; set; }
    }
}
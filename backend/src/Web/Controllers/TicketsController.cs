using AutoMapper;
using EventManagement.ApplicationCore.Auditing;
using EventManagement.ApplicationCore.Interfaces;
using EventManagement.Identity;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Extensions;
using EventManagement.WebApp.Models;
using Fop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class TicketsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITicketNumberService _ticketNumberService;
        private readonly IAuditEventLog _auditEventLog;

        public TicketsController(EventsDbContext context,
                                 IMapper mapper,
                                 ITicketNumberService ticketNumberService,
                                 IAuditEventLog auditEventLog)
        {
            _context = context;
            _mapper = mapper;
            _ticketNumberService = ticketNumberService;
            _auditEventLog = auditEventLog;
        }

        /// <summary>
        /// Lists all tickets for a given event.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="query">Filter tickets by any criteria.</param>
        /// <param name="isDelivered">if true, only delivered tickets are listed.</param>
        /// <param name="validated">if true, which have gone through entrance control successfully will be listed.</param>
        /// <param name="ticketTypeId">Filter the list by a specific ticket type.</param>
        [HttpGet("events/{eventId}/tickets")]
        public ActionResult<PaginationResult<Ticket>> GetTickets(Guid eventId, [FromQuery] FopQuery query,
                                                                 bool? isDelivered, bool? validated,
                                                                 Guid? ticketTypeId)
        {
            var tickets = _context.Tickets
                .AsNoTracking()
                .Include(t => t.TicketType)
                .Where(e => e.EventId == eventId);

            if (isDelivered != null)
            {
                tickets = tickets.Where(e => e.IsDelivered == isDelivered.Value);
            }
            if (validated != null)
            {
                tickets = tickets.Where(e => e.Validated == validated.Value);
            }
            if (ticketTypeId != null)
            {
                tickets = tickets.Where(e => e.TicketTypeId == ticketTypeId.Value);
            }

            tickets = tickets.OrderByDescending(x => x.CreatedAt);

            return _mapper
                .ProjectTo<Ticket>(tickets)
                .ApplyQuery(query);
        }

        /// <summary>
        /// Get a single ticket by its id.
        /// </summary>
        /// <param name="id">Id of the ticket.</param>
        /// <returns>ticket details</returns>
        [HttpGet("tickets/{id}")]
        public ActionResult<Ticket> GetById(Guid id)
        {
            var entity = _context.Tickets.Find(id);
            return _mapper.Map<Ticket>(entity);
        }

        /// <summary>
        /// Create a new ticket.
        /// </summary>
        /// <param name="model">ticket details</param>
        /// <returns>details of created ticket.</returns>
        [HttpPost("tickets")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<Ticket>> CreateTicketAsync(Ticket model)
        {
            if (model.Id != Guid.Empty)
                return BadRequest(
                    new ProblemDetails { Detail = "This method can't be used to update tickets." });
            var evt = _context.Events.Find(model.EventId);
            if (evt == null)
                return BadRequest(
                    new ProblemDetails { Detail = $"There's no event with id {model.EventId}." });
            var entity = new ApplicationCore.Models.Ticket();
            _mapper.Map(model, entity);
            entity.TicketSecret = Guid.NewGuid().ToString("N");
            entity.TicketNumber = entity.TicketNumber
                ?? _ticketNumberService.GenerateTicketNumber(evt);
            SetAuthorInfo(entity);
            _context.Add(entity);
            _context.SaveChanges();
            _context.Entry(entity).Reference(e => e.TicketType).Load();

            if (entity.BookingDate != null)
            {
                await _auditEventLog.AddAsync(new ApplicationCore.Models.AuditEvent
                {
                    Time = entity.BookingDate.Value,
                    TicketId = entity.Id,
                    Action = EventManagementConstants.Auditing.Actions.TicketOrder,
                    Detail = $"Ticket der Kategorie {entity.TicketType.Name} wurde für {entity.TicketType.Price:c} bestellt.",
                    Succeeded = true
                });
            }

            model = _mapper.Map<Ticket>(entity);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        /// <summary>
        /// Update all details of an existing ticket (replace all fields).
        /// </summary>
        /// <param name="id">Id of the ticket.</param>
        /// <param name="model">Ticket details.</param>
        /// <returns>updated ticket.</returns>
        [HttpPut("tickets/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public ActionResult UpdateTicket(Guid id, [FromBody] Ticket model)
        {
            if (id != model.Id)
                return BadRequest(new ProblemDetails { Detail = "wrong id" });
            var entity = _context.Tickets.Find(model.Id);
            if (entity == null)
                return NotFound();
            if (model.TicketNumber != entity.TicketNumber)
                return BadRequest(
                    new ProblemDetails { Detail = "The TicketNumber can't be changed." });
            if (model.EventId != entity.EventId)
                return BadRequest(
                    new ProblemDetails { Detail = "The ticket is only valid for a single event." });
            _mapper.Map(model, entity);
            SetAuthorInfo(entity);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Apply some changes to an existing ticket.
        /// </summary>
        /// <param name="id">Id of the ticket.</param>
        /// <param name="patchDoc">JSON Patch Document which describes the changes.
        /// See <see href="http://jsonpatch.com"/></param>
        /// <returns></returns>
        [HttpPatch("tickets/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public ActionResult UpdateTicketPatch(Guid id, [FromBody] JsonPatchDocument<Ticket> patchDoc)
        {
            var entity = _context.Tickets.Find(id);
            if (entity == null)
                return NotFound();
            var model = _mapper.Map<Ticket>(entity);
            patchDoc.ApplyTo(model);
            if (model.TicketNumber != entity.TicketNumber)
                return BadRequest(
                    new ProblemDetails { Detail = "The TicketNumber can't be changed." });
            if (model.EventId != entity.EventId)
                return BadRequest(
                    new ProblemDetails { Detail = "The ticket is only valid for a single event." });
            _mapper.Map(model, entity);
            SetAuthorInfo(entity);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Delete a specific ticket.
        /// </summary>
        /// <param name="id">Id of the ticket.</param>
        [HttpDelete("tickets/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public IActionResult DeleteTicket(Guid id)
        {
            var entity = _context.Tickets.Find(id);
            if (entity == null)
                return NotFound();
            entity.IsDeleted = true;
            SetAuthorInfo(entity);
            _context.SaveChanges();
            return Ok();
        }

        private void SetAuthorInfo(ApplicationCore.Models.Ticket entity)
        {
            if (User.IsPerson())
            {
                Guid currentUserId = User.GetUserId();
                entity.EditorId = currentUserId;
                if (entity.Id == Guid.Empty)
                    entity.CreatorId = currentUserId;
            }
            else
            {
                // API Client (S2S) without user.
                // TODO: Save client id as author information.
                entity.EditorId = null;
            }
            var timestamp = DateTime.UtcNow;
            entity.EditedAt = timestamp;
            if (entity.Id == Guid.Empty)
                entity.CreatedAt = timestamp;
        }
    }
}
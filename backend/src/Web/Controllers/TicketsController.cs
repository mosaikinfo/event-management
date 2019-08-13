using AutoMapper;
using EventManagement.ApplicationCore.Interfaces;
using EventManagement.Identity;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static IdentityServer4.IdentityServerConstants;

namespace EventManagement.WebApp.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = LocalApi.AuthenticationScheme)]
    public class TicketsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITicketNumberService _ticketNumberService;

        public TicketsController(EventsDbContext context,
                                 IMapper mapper,
                                 ITicketNumberService ticketNumberService)
        {
            _context = context;
            _mapper = mapper;
            _ticketNumberService = ticketNumberService;
        }

        /// <summary>
        /// Lists all tickets for a given event.
        /// </summary>
        [HttpGet("events/{eventId}/tickets")]
        public ActionResult<IList<Ticket>> GetTickets(Guid eventId, string filter, string filterValue)
        {
            IQueryable<ApplicationCore.Models.Ticket> query =
                _context.Tickets
                    .AsNoTracking()
                    .Where(e => e.EventId == eventId)
                    .OrderByDescending(x => x.CreatedAt);
            if (filter != null)
            {
                filter = filter.ToLowerInvariant();
                if (filter == "ticketnumber")
                {
                    query = query.Where(e => e.TicketNumber == filterValue);
                }
                else
                {
                    return BadRequest(
                        new ProblemDetails { Detail = "Invalid value for parameter 'filter'." });
                }
            }
            return query.Select(_mapper.Map<Ticket>).ToList();
        }

        [HttpGet("tickets/{id}")]
        public ActionResult<Ticket> GetById(Guid id)
        {
            var entity = _context.Tickets.Find(id);
            return _mapper.Map<Ticket>(entity);
        }

        [HttpPost("tickets")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public ActionResult<Ticket> CreateTicket(Ticket model)
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
            model = _mapper.Map<Ticket>(entity);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut("tickets/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Put))]
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

        [HttpDelete("tickets/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
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
            var timestamp = DateTime.UtcNow;
            Guid currentUserId = User.GetUserId();
            entity.EditedAt = timestamp;
            entity.EditorId = currentUserId;
            if (entity.Id == Guid.Empty)
            {
                entity.CreatorId = currentUserId;
                entity.CreatedAt = timestamp;
            }
        }
    }
}
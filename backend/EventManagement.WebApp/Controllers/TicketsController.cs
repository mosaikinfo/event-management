using AutoMapper;
using EventManagement.DataAccess;
using EventManagement.Identity;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventManagement.WebApp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = Constants.JwtAuthScheme)]
    public class TicketsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public TicketsController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all tickets for a given event.
        /// </summary>
        [HttpGet("api/events/{eventId}/tickets")]
        public IEnumerable<Ticket> GetTickets(int eventId)
        {
            return _context.Tickets
                .AsNoTracking()
                .Where(e => e.EventId == eventId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(_mapper.Map<Ticket>);
        }

        [HttpGet("api/tickets/{id}")]
        public ActionResult<Ticket> GetById(int id)
        {
            var entity = _context.Tickets.Find(id);
            return _mapper.Map<Ticket>(entity);
        }

        [HttpPost("api/tickets")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public ActionResult<Ticket> CreateTicket(Ticket model)
        {
            if (model.Id > 0)
                return BadRequest(
                    new ProblemDetails { Detail = "This method can't be used to update tickets." });
            if (model.EventId <= 0)
                return BadRequest(
                    new ProblemDetails { Detail = "The field EventId is required." });
            var evt = _context.Events.Find(model.EventId);
            if (evt == null)
                return BadRequest(
                    new ProblemDetails { Detail = $"There's no event with id {model.EventId}." });
            var entity = new DataAccess.Models.Ticket();
            _mapper.Map(model, entity);
            entity.TicketGuid = Guid.NewGuid();
            entity.TicketNumber = entity.TicketNumber
                ?? TicketNumberHelper.GenerateTicketNumber(evt);
            SetAuthorInfo(entity);
            _context.Add(entity);
            _context.SaveChanges();
            model = _mapper.Map<Ticket>(entity);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut("api/tickets/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Put))]
        public ActionResult UpdateTicket(int id, [FromBody] Ticket model)
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

        private void SetAuthorInfo(DataAccess.Models.Ticket entity)
        {
            var timestamp = DateTime.UtcNow;
            int currentUserId = User.GetUserId();
            entity.EditedAt = timestamp;
            entity.EditorId = currentUserId;
            if (entity.Id <= 0)
            {
                entity.CreatorId = currentUserId;
                entity.CreatedAt = timestamp;
            }
        }
    }
}
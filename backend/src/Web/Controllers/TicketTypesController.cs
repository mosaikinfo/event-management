using AutoMapper;
using EventManagement.Infrastructure.Data;
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
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class TicketTypesController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public TicketTypesController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// List all available ticket types for an event.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <returns>List of ticket types.</returns>
        [HttpGet("events/{eventId}/tickettypes")]
        public ActionResult<IList<TicketType>> GetTicketTypes(Guid eventId)
        {
            return _context.TicketTypes
                .AsNoTracking()
                .Where(e => e.EventId == eventId)
                .ToList()
                .Select(_mapper.Map<TicketType>)
                .ToList();
        }

        /// <summary>
        /// Set all available ticket types for an event.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="items">Ticket types to set for the event.</param>
        /// <returns>List of ticket types.</returns>
        [HttpPost("events/{eventId}/tickettypes")]
        public ActionResult<IList<TicketType>> AddOrUpdateTicketTypes(Guid eventId, [FromBody] TicketType[] items)
        {
            var evt = _context.Events
                .Include(e => e.TicketTypes)
                .SingleOrDefault(e => e.Id == eventId);
            if (evt == null)
                return NotFound();
            // Delete ticket types.
            foreach (ApplicationCore.Models.TicketType ticketType in evt.TicketTypes)
            {
                if (items.All(t => t.Id != ticketType.Id))
                {
                    _context.Entry(ticketType).State = EntityState.Deleted;
                }
            }
            var list = new List<TicketType>();
            foreach (TicketType item in items)
            {
                if (item.Id != Guid.Empty)
                {
                    // Update ticket type.
                    var entity = evt.TicketTypes.SingleOrDefault(t => t.Id == item.Id);
                    if (entity == null)
                    {
                        return BadRequest(
                            new ProblemDetails { Detail = $"There is no ticket type with id {item.Id}." });
                    }
                    else
                    {
                        _mapper.Map(item, entity);
                        _context.SaveChanges();
                        list.Add(_mapper.Map<TicketType>(entity));
                    }
                }
                else
                {
                    // Create new ticket type.
                    var entity = _mapper.Map<ApplicationCore.Models.TicketType>(item);
                    evt.TicketTypes.Add(entity);
                    _context.SaveChanges();
                    list.Add(_mapper.Map<TicketType>(entity));
                }
            }
            _context.SaveChanges();
            return list;
        }
    }
}
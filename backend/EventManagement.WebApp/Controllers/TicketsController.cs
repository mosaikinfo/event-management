using AutoMapper;
using EventManagement.DataAccess;
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
        public ActionResult<Ticket> CreateTicket(Ticket model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("api/tickets/{id}")]
        public ActionResult UpdateTicket(int id, [FromBody] Ticket model)
        {
            if (id != model.Id)
                return BadRequest();

            throw new NotImplementedException();
        }
    }
}
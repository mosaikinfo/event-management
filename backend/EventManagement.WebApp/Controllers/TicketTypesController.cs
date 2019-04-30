using AutoMapper;
using EventManagement.DataAccess;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EventManagement.WebApp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = Constants.JwtAuthScheme)]
    public class TicketTypesController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public TicketTypesController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("api/event/{eventId}/tickettypes")]
        public ActionResult<IList<TicketType>> GetTicketTypes(int eventId)
        {
            return _context.Query<DataAccess.Models.TicketType>()
                .AsNoTracking()
                .Where(e => e.EventId == eventId)
                .ToList()
                .Select(_mapper.Map<TicketType>)
                .ToList();
        }

        [HttpPost("api/event/{eventId}/tickettypes")]
        public ActionResult<TicketType> CreateTicketType(int eventId, [FromBody] TicketType model)
        {
            var entity = _mapper.Map<DataAccess.Models.TicketType>(model);
            entity.EventId = eventId;
            _context.Add(entity);
            _context.SaveChanges();
            return _mapper.Map<TicketType>(entity);
        }

        [HttpPut("api/event/{eventId}/tickettypes/{id}")]
        public ActionResult UpdateTicketType(int eventId, int id, [FromBody] TicketType model)
        {
            if (model.Id != id)
                return BadRequest();
            var entity = _context.Find<DataAccess.Models.TicketType>(id);
            if (entity == null || entity.EventId != eventId)
                return NotFound();
            _mapper.Map(model, entity);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
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
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = Constants.JwtAuthScheme)]
    public class EventsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public EventsController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Event> GetAll()
        {
            return _context.Events
                .AsNoTracking()
                .OrderBy(x => x.StartTime)
                .Select(_mapper.Map<Event>);
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            // TODO: validate permissions.
            return _context.Events
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(_mapper.Map<Event>)
                .FirstOrDefault();
        }

        [HttpPost]
        public ActionResult<Event> CreateEvent([FromBody] Event model)
        {
            // TODO: validate permissions.
            if (model.Id > 0)
                return BadRequest();
            var entity = new DataAccess.Models.Event();
            _mapper.Map(model, entity);
            _context.Add(entity);
            _context.SaveChanges();
            return _mapper.Map<Event>(entity);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateEvent(int id, [FromBody] Event model)
        {
            // TODO: validate permissions.
            if (id != model.Id)
                return BadRequest();
            var entity = _context.Events.Find(model.Id);
            if (entity == null)
                return NotFound();
            _mapper.Map(model, entity);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
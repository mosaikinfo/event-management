using EventManagement.DataAccess;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
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

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Event> GetAll()
        {
            return _context.Events
                .AsNoTracking()
                .OrderBy(x => x.StartTime)
                .Select(CreateViewModel);
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            // TODO: validate permissions.
            return _context.Events
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(CreateViewModel)
                .FirstOrDefault();
        }

        [HttpPost]
        public ActionResult<Event> CreateEvent([FromBody] Event model)
        {
            // TODO: validate permissions.
            if (model.Id > 0)
                return BadRequest();
            var entity = new DataAccess.Models.Event();
            Map(model, entity);
            _context.Add(entity);
            _context.SaveChanges();
            return CreateViewModel(entity);
        }

        [HttpPut("{id}")]
        public ActionResult<Event> UpdateEvent(int id, [FromBody] Event model)
        {
            // TODO: validate permissions.
            if (id != model.Id)
                return BadRequest();
            var entity = _context.Events.Find(model.Id);
            if (entity == null)
                return NotFound();
            Map(model, entity);
            _context.SaveChanges();
            return NoContent();
        }

        private Event CreateViewModel(DataAccess.Models.Event source)
        {
            return new Event
            {
                Id = source.Id,
                Name = source.Name,
                StartTime = source.StartTime,
                EndTime = source.EndTime,
                EntranceTime = source.EntranceTime,
                Location = source.Location
            };
        }

        private void Map(Event source, DataAccess.Models.Event destination)
        {
            destination.Name = source.Name;
            destination.StartTime = source.StartTime;
            destination.EndTime = source.EndTime;
            destination.EntranceTime = source.EntranceTime;
            destination.Location = source.Location;
        }
    }
}
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
    [Route("api/[controller]")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class EventsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public EventsController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// List events.
        /// </summary>
        [HttpGet]
        public IEnumerable<Event> GetAll()
        {
            return _context.Events
                .AsNoTracking()
                .OrderBy(x => x.StartTime)
                .Select(_mapper.Map<Event>);
        }

        /// <summary>
        /// Get an event by its id.
        /// </summary>
        /// <param name="id">Id of the event.</param>
        /// <returns>the event.</returns>
        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(Guid id)
        {
            return _context.Events
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(_mapper.Map<Event>)
                .FirstOrDefault();
        }

        /// <summary>
        /// Create a new event.
        /// </summary>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public ActionResult<Event> CreateEvent([FromBody] Event model)
        {
            if (model.Id != Guid.Empty)
                return BadRequest();
            var entity = new ApplicationCore.Models.Event();
            _mapper.Map(model, entity);
            _context.Add(entity);
            _context.SaveChanges();
            model = _mapper.Map<Event>(entity);
            return CreatedAtAction(nameof(GetEvent), new { id = model.Id }, model);
        }

        /// <summary>
        /// Update an event.
        /// </summary>
        /// <param name="id">Id of the event.</param>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public ActionResult UpdateEvent(Guid id, [FromBody] Event model)
        {
            if (id != model.Id)
                return BadRequest();
            var entity = _context.Events.Find(model.Id);
            if (entity == null)
                return NotFound();
            _mapper.Map(model, entity);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Delete an event.
        /// </summary>
        /// <param name="id">Id of the event.</param>
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public IActionResult DeleteEvent(Guid id)
        {
            var entity = _context.Events.Find(id);
            if (entity == null)
                return NotFound();
            entity.IsDeleted = true;
            _context.SaveChanges();
            return Ok();
        }
    }
}
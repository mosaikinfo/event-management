using AutoMapper;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to manage api clients for an event.
    /// </summary>
    [ApiController]
    [Route("clients")]
    [Authorize(Constants.EventManagementApiPolicy)]
    public class ClientsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public ClientsController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all api clients.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Client>> GetClientsOfEvent()
        {
            List<ApplicationCore.Models.Client> clients =
                await _context.Clients
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

            return clients.Select(_mapper.Map<Client>);
        }

        /// <summary>
        /// Get a single client by its id.
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>client details</returns>
        [HttpGet("{id}")]
        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<Client>(client);
        }

        /// <summary>
        /// Add a new api client.
        /// </summary>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public ActionResult<Client> CreateClient([FromBody] Client model)
        {
            if (model.Id != Guid.Empty)
                return BadRequest();
            var entity = _mapper.Map<ApplicationCore.Models.Client>(model);
            entity.CreatedAt = DateTime.UtcNow;
            _context.Add(entity);
            _context.SaveChanges();
            model = _mapper.Map<Client>(entity);
            return CreatedAtAction(nameof(GetClientByIdAsync), new { id = model.Id }, model);
        }

        /// <summary>
        /// Update a certain api client.
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="model">properties to update</param>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public ActionResult UpdateClient(Guid id, [FromBody] Client model)
        {
            if (id != model.Id)
                return BadRequest();
            var entity = _context.Clients.Find(model.Id);
            if (entity == null)
                return NotFound();
            _mapper.Map(model, entity);
            entity.EditedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
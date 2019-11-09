using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class EventStatusController : ControllerBase
    {
        private readonly EventsDbContext _context;

        public EventStatusController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Report the status of the event (sent tickets, checked-in tickets).
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        [Route("events/{eventId}/status")]
        public Task<EventStatus> GetStatusAsync(Guid eventId)
        {
            return _context.Events
                .Where(e => e.Id == eventId)
                .Select(e => new EventStatus
                {
                    TicketsTotal = e.Tickets.Count(),
                    TicketsDelivered = e.Tickets.Count(t => t.IsDelivered),
                    TicketsCheckedIn = e.Tickets.Count(t => t.Validated)
                })
                .SingleOrDefaultAsync();
        }
    }
}
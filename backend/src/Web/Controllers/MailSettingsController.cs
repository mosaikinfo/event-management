using AutoMapper;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class MailSettingsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public MailSettingsController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get mail settings for an event.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <returns>Mail settings</returns>
        [HttpGet("events/{eventId}/mailsettings")]
        public async Task<MailSettings> GetMailSettingsAsync(Guid eventId)
        {
            var mailSettings = await _context.MailSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Event.Id == eventId)
                ?? new ApplicationCore.Models.MailSettings();

            return _mapper.Map<MailSettings>(mailSettings);
        }

        /// <summary>
        /// Update mail settings for an event.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="values">mail settings</param>
        [HttpPost("events/{eventId}/mailsettings")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult> UpdateMailSettingsAsync(Guid eventId, MailSettings values)
        {
            var evt = await _context.Events
                .Include(e => e.MailSettings)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (evt == null)
                return BadRequest(
                    new ProblemDetails { Detail = $"There's no event with id {eventId}." });

            if (evt.MailSettings == null)
                evt.MailSettings = new ApplicationCore.Models.MailSettings();

            _mapper.Map(values, evt.MailSettings);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
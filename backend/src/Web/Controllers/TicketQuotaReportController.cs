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
    /// API to report the sold tickets and quotas for each ticket type.
    /// </summary>
    [ApiController]
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class TicketQuotaReportController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public TicketQuotaReportController(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Report the sold tickets and quotas for each ticket type.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        [Route("events/{eventId}/reports/quotas")]
        public Task<List<TicketQuotaReportRow>> GetReport(Guid eventId)
        {
            var query = _context.TicketTypes
                .AsNoTracking()
                .Where(e => e.EventId == eventId);

            return _mapper
                .ProjectTo<TicketQuotaReportRow>(query)
                .ToListAsync();
        }
    }
}
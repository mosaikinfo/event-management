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
    /// <summary>
    /// Audit event log
    /// </summary>
    [ApiController]
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class AuditEventsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public AuditEventsController(EventsDbContext context,
                                 IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// List audit event log entries for a specific ticket.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        /// <returns>List of audit events. Recent entries at first.</returns>
        [HttpGet("tickets/{ticketId}/auditevents")]
        public IList<AuditEvent> List(Guid ticketId)
        {
            return _context.AuditEventLog
                .AsNoTracking()
                .Where(x => x.TicketId == ticketId)
                .OrderByDescending(x => x.Time)
                .Select(_mapper.Map<AuditEvent>)
                .ToList();
        }
    }
}
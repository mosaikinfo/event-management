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
    /// API to manage support tickets.
    /// Support tickets are issued to people who wait in the support line.
    /// </summary>
    [ApiController]
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class SupportTicketController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public SupportTicketController(EventsDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("events/{eventId}/supporttickets")]
        public ActionResult<List<SupportTicket>> ListSupportTickets(Guid eventId,
            ApplicationCore.Models.SupportTicketStatus? status)
        {
            IEnumerable<ApplicationCore.Models.SupportTicket> supportTickets =
                _context.SupportTickets
                    .AsNoTracking()
                    .Include(e => e.Ticket)
                    .Where(e => e.Ticket.EventId == eventId)
                    .OrderByDescending(e => e.CreatedAt);

            if (status != null)
            {
                supportTickets = supportTickets.Where(e => e.Status == status);
            }

            if (status == ApplicationCore.Models.SupportTicketStatus.Closed)
            {
                supportTickets = supportTickets.OrderByDescending(e => e.ClosedAt);
            }

            return supportTickets
                .Select(_mapper.Map<SupportTicket>)
                .ToList();
        }
    }
}
using AutoMapper;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// API to manage support tickets.
    /// Support tickets are issued to people who wait in the support line.
    /// </summary>
    [ApiController]
    [Route("api")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class SupportTicketsController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;

        public SupportTicketsController(EventsDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Lists all support tickets for a given event.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="status">Filter the support tickets by status (new, inprogress, closed).</param>
        [HttpGet("events/{eventId}/supporttickets")]
        public ActionResult<List<SupportTicket>> List(Guid eventId,
            ApplicationCore.Models.SupportTicketStatus? status)
        {
            IEnumerable<ApplicationCore.Models.SupportTicket> supportTickets =
                _context.SupportTickets
                    .AsNoTracking()
                    .Include(e => e.Ticket)
                    .Where(e => e.Ticket.EventId == eventId)
                    .OrderBy(e => e.CreatedAt);

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

        /// <summary>
        /// Update the status of a support ticket.
        /// </summary>
        /// <param name="id">Id of the ticket.</param>
        /// <param name="args">new status of the ticket.</param>
        [HttpPost("/supporttickets/{id}/status")]
        public async Task<IActionResult> SetStatusAsync(Guid id, SetStatusCommandArgs args)
        {
            var supportTicket = await _context.SupportTickets.FindAsync(id);

            if (supportTicket == null)
                return NotFound();

            if (supportTicket.Status != args.NewStatus)
            {
                supportTicket.Status = args.NewStatus;

                if (args.NewStatus == ApplicationCore.Models.SupportTicketStatus.Closed)
                {
                    supportTicket.ClosedAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
    }

    public class SetStatusCommandArgs
    {
        [Required]
        public ApplicationCore.Models.SupportTicketStatus NewStatus { get; set; }
    }
}
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketSupport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Data.Repositories
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        private readonly EventsDbContext _context;

        public SupportTicketRepository(EventsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<SupportTicket> CreateSupportTicketAsync(Guid ticketId, string description)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                throw new DbQueryException("Ticket not found");

            var supportTicket = new SupportTicket
            {
                Ticket = ticket,
                Description = description,
                Status = SupportTicketStatus.New,
                CreatedAt = DateTime.UtcNow
            };
            supportTicket.SupportNumber = await GetNextSupportNumberAsync(ticket.EventId);
            await _context.AddAsync(supportTicket);
            await _context.SaveChangesAsync();
            return supportTicket;
        }

        private async Task<int> GetNextSupportNumberAsync(Guid eventId)
        {
            int lastNumber = await _context.SupportTickets
                .Where(e => e.Ticket.EventId == eventId)
                .OrderByDescending(e => e.SupportNumber)
                .Select(e => e.SupportNumber)
                .FirstOrDefaultAsync();

            return lastNumber + 1;
        }
    }
}
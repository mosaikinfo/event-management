using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Tickets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Data.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly EventsDbContext _context;

        public TicketsRepository(EventsDbContext context)
        {
            _context = context;
        }

        public Task<bool> ExistsAsync(Guid ticketId)
        {
            return _context.Tickets.AnyAsync(t => t.Id == ticketId);
        }

        public Task<Ticket> GetAsync(Guid ticketId)
        {
            return _context.Tickets
                .Include(x => x.Event)
                .Include(x => x.TicketType)
                .SingleOrDefaultAsync(x => x.Id == ticketId);
        }
    }
}
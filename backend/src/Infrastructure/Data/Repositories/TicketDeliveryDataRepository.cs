using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Data.Repositories
{
    public class TicketDeliveryDataRepository : ITicketDeliveryDataRepository
    {
        private readonly EventsDbContext _context;

        public TicketDeliveryDataRepository(EventsDbContext context)
        {
            _context = context;
        }

        public Task<bool> Exists(Guid ticketId)
        {
            return _context.Tickets.AnyAsync(t => t.Id == ticketId);
        }

        public async Task<TicketDeliveryData> GetAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(e => e.Event)
                .ThenInclude(e => e.MailSettings)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                return null;

            return new TicketDeliveryData
            {
                Ticket = ticket,
                MailSettings = ticket.Event.MailSettings
            };
        }
    }
}

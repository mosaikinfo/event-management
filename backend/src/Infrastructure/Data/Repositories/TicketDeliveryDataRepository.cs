using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketDelivery;
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

        public async Task<TicketDeliveryData> GetAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .AsNoTracking()
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
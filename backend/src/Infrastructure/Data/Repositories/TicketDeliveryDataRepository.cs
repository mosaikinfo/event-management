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

        public async Task UpdateDeliveryStatusAsync(Guid ticketId, bool isDelivered, DateTime deliveryDate, TicketDeliveryType deliveryType)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                throw new DbQueryException("Ticket not found");

            ticket.IsDelivered = isDelivered;
            ticket.DeliveryDate = deliveryDate;
            ticket.DeliveryType = deliveryType;
            await _context.SaveChangesAsync();
        }
    }
}
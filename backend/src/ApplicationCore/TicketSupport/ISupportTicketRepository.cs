using EventManagement.ApplicationCore.Models;
using System;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketSupport
{
    public interface ISupportTicketRepository
    {
        Task<SupportTicket> CreateSupportTicketAsync(Guid ticketId, string description);
    }
}
using EventManagement.ApplicationCore.Models;
using System;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Tickets
{
    public interface ITicketsRepository
    {
        Task<bool> ExistsAsync(Guid ticketId);

        Task<Ticket> GetAsync(Guid ticketId);
    }
}
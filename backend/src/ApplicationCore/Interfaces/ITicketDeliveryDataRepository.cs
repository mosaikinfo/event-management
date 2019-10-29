using EventManagement.ApplicationCore.Models;
using System;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Interfaces
{
    public interface ITicketDeliveryDataRepository
    {
        Task<bool> Exists(Guid ticketId);

        Task<TicketDeliveryData> GetAsync(Guid ticketId);
    }
}

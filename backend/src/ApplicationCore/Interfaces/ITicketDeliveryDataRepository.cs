using EventManagement.ApplicationCore.Models;
using System;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Interfaces
{
    public interface ITicketDeliveryDataRepository
    {
        Task<TicketDeliveryData> GetAsync(Guid ticketId);
    }
}

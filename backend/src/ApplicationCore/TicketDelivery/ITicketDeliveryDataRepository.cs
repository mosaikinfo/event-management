using EventManagement.ApplicationCore.Models;
using System;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    public interface ITicketDeliveryDataRepository
    {
        Task<TicketDeliveryData> GetAsync(Guid ticketId);
    }
}
using System;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Tickets
{
    public interface ITicketRedirectService
    {
        Task<string> GetRedirectUrlAsync(Guid ticketId, string ticketValidationUrl);
    }
}
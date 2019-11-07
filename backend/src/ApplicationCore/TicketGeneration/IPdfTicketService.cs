using System;
using System.IO;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketGeneration
{
    /// <summary>
    /// Service to generate printable tickets as pdf.
    /// </summary>
    public interface IPdfTicketService
    {
        /// <summary>
        /// Generate the pdf and return as file stream.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        /// <returns>file stream</returns>
        Task<Stream> GeneratePdfAsync(Guid ticketId, string ticketValidationUriFormat);
    }
}
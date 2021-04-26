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
        /// <param name="ticketValidationUriFormat">
        /// An URI template of the validation url with the placeholder
        /// <see cref="EventManagementConstants.TicketGeneration.SecretUrlPlaceholder"/>.
        /// Example: <c>https://mydomain.com/v/--secret--</c></param>
        /// <returns>file stream</returns>
        Task<Stream> GeneratePdfAsync(Guid ticketId, string ticketValidationUriFormat);
    }
}

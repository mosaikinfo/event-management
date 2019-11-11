using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.Models;
using System;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    /// <summary>
    /// Service for the delivery of tickets to the buyer by various ways (e-mail, SMS, letter post, etc.).
    /// </summary>
    public interface ITicketDeliveryService
    {
        /// <summary>
        /// Validate whether the ticket can be delivered to the buyer.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        /// <param name="deliveryType">Delivery type (e-mail, SMS, letter post, etc.).</param>
        /// <exception cref="NotSupportedException">when the delivery type is not yet supported.</exception>
        /// <exception cref="TicketNotFoundException">when the requested ticket doesn't exist.</exception>
        Task ValidateAsync(Guid ticketId, TicketDeliveryType deliveryType);

        /// <summary>
        /// Deliver a ticket to the buyer.
        /// </summary>
        /// <param name="ticketId">Id of the ticket.</param>
        /// <param name="deliveryType">Delivery type (e-mail, SMS, letter post, etc.).</param>
        /// <param name="homepageUrl">URL to redirect the users to that scan the qr code at themselves.</param>
        /// <exception cref="NotSupportedException">when the delivery type is not yet supported.</exception>
        /// <exception cref="TicketNotFoundException">when the requested ticket doesn't exist.</exception>
        Task SendTicketAsync(Guid ticketId, TicketDeliveryType deliveryType,
                             string ticketValidationUriFormat, string homepageUrl);
    }
}
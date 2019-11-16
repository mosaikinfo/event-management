using System;
using System.Collections.Generic;

namespace EventManagement.WebApp.Models
{
    /// <summary>
    /// Specification to query a subset of tickets you want to send via e-mail.
    /// </summary>
    public class TicketsSendSpecification
    {
        /// <summary>
        /// Allows sending e-mails twice (even if the ticket has been sent before).
        /// </summary>
        public bool SendAll { get; set; }

        /// <summary>
        /// Ticket types to filter by.
        /// </summary>
        public List<Guid> TicketTypes { get; set; }

        /// <summary>
        /// Try the method without sending e-mails.
        /// </summary>
        public bool DryRun { get; set; }
    }
}
using System;

namespace EventManagement.ApplicationCore.Models
{
    public class AuditEvent : BaseEntity
    {
        public DateTime Time { get; set; }

        public string Action { get; set; }

        public string Detail { get; set; }

        public bool Succeeded { get; set; }

        public Guid TicketId { get; set; }

        public Ticket Ticket { get; set; }
    }
}
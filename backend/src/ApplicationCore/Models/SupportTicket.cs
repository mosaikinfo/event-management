using System;

namespace EventManagement.ApplicationCore.Models
{
    public class SupportTicket : BaseEntity
    {
        public Guid TicketId { get; set; }

        public int SupportNumber { get; set; }

        public string Description { get; set; }

        public SupportTicketStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public Ticket Ticket { get; set; }
    }
}
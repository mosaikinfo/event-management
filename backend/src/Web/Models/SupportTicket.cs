using System;

namespace EventManagement.WebApp.Models
{
    public class SupportTicket
    {
        public Guid Id { get; set; }

        public Guid TicketId { get; set; }

        public string TicketNumber { get; set; }

        public int SupportNumber { get; set; }

        public string Description { get; set; }

        public ApplicationCore.Models.SupportTicketStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}
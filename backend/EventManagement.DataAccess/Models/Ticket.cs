using System;

namespace EventManagement.DataAccess.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
        public Guid TicketGuid { get; set; }
        public int EventId { get; set; }
        public int TicketTypeId { get; set; }
        public bool Validated { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public bool? TermsAccepted { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public int? CreatorId { get; set; }
        public int? EditorId { get; set; }
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
        public TicketType TicketType { get; set; }
        public User Creator { get; set; }
        public User Editor { get; set; }
    }
}
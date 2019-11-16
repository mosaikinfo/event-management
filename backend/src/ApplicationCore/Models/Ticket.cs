using System;

namespace EventManagement.ApplicationCore.Models
{
    public class Ticket : BaseEntity
    {
        public string TicketNumber { get; set; }
        public string TicketSecret { get; set; }
        public Guid EventId { get; set; }
        public Guid TicketTypeId { get; set; }
        public bool Validated { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal? AmountPaid { get; set; }
        public bool? TermsAccepted { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? EditorId { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public TicketDeliveryType? DeliveryType { get; set; }
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
        public TicketType TicketType { get; set; }
        public User Creator { get; set; }
        public User Editor { get; set; }
    }
}
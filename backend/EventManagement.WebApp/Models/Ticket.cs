using EventManagement.DataAccess.Models;
using System;

namespace EventManagement.WebApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
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

        public string Creator { get; set; }
        public string Editor { get; set; }
    }
}
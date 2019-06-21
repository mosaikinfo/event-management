using EventManagement.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.WebApp.Models
{
    public class Ticket : IValidatableObject
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
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

        public string Creator { get; set; }
        public string Editor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventId <= 0)
                yield return new ValidationResult(
                    "The field is required.", new[] { nameof(EventId) });
            if (TicketTypeId <= 0)
                yield return new ValidationResult(
                    "The field is required.", new[] { nameof(TicketTypeId) });
        }
    }
}
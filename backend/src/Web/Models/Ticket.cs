using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Models.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.WebApp.Models
{
    public class Ticket : IValidatableObject
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; }
        public Guid EventId { get; set; }
        public Guid TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public bool Validated { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public float? AmountPaid { get; set; }
        public bool TermsAccepted { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string RoomNumber { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }

        public string Creator { get; set; }
        public string Editor { get; set; }

        public bool IsDelivered { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public TicketDeliveryType? DeliveryType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventId == Guid.Empty)
                yield return new ValidationResult(
                    "The field is required.", new[] { nameof(EventId) });

            if (TicketTypeId == Guid.Empty)
                yield return new ValidationResult(
                    "The field is required.", new[] { nameof(TicketTypeId) });

            if (Gender != null && GenderExtensions.FromStringValue(Gender) == null)
                yield return new ValidationResult(
                    $"Gender '{Gender}' is not an allowed value. Allowed values: 'm' or 'f'.",
                    new[] { nameof(Gender) });

            if (BookingDate != null && BookingDate > DateTime.UtcNow)
                yield return new ValidationResult(
                    "The BookingDate must be in the past.", new[] { nameof(BookingDate) });
        }
    }
}
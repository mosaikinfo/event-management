using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.WebApp.Models
{
    public class Event : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public DateTime? EntranceTime { get; set; }

        public string Location { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime > EndTime)
            {
                yield return new ValidationResult("The start time must be before the end time.");
            }
        }
    }
}

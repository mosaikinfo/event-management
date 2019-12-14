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

        [Required]
        public string Location { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string HomepageUrl { get; set; }

        /// <summary>
        /// True, if you want to add personal information in form 
        /// of a JSON Web Token (JWT) when redirecting to the homepage.
        /// </summary>
        public bool IncludePersonalInformation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime > EndTime)
            {
                yield return new ValidationResult("The start time must be before the end time.");
            }
        }
    }
}
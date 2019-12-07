using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    public class EmailMessage : IValidatableObject
    {
        public IList<string> From { get; set; } = new List<string>();

        public IList<string> To { get; set; } = new List<string>();

        public IList<string> ReplyTo { get; set; } = new List<string>();

        public string Subject { get; set; }

        public string Body { get; set; }

        public IList<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From == null || From.Count == 0)
            {
                yield return new ValidationResult("At least one e-mail sender (From) is required.");
            }
            if (To == null || To.Count == 0)
            {
                yield return new ValidationResult("At least one e-mail recipient (To) is required.");
            }
        }
    }
}
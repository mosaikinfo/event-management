using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventManagement.WebApp.Models
{
    public class MailSettings : IValidatableObject
    {
        [Required]
        public string SmtpHost { get; set; }

        [Range(1, 65535)]
        public int SmtpPort { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public bool UseStartTls { get; set; }

        [Required]
        public string SenderAddress { get; set; }

        public string ReplyToAddress { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public bool EnableDemoMode { get; set; }

        public IList<string> DemoEmailRecipients { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DemoEmailRecipients?.Any() == true &&
                DemoEmailRecipients.Any(e => string.IsNullOrEmpty(e?.Trim())))
            {
                yield return new ValidationResult(
                        "The email address may not be empty.", new[] { nameof(DemoEmailRecipients) });
            }
        }
    }
}
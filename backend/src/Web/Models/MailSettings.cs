using System.ComponentModel.DataAnnotations;

namespace EventManagement.WebApp.Models
{
    public class MailSettings
    {
        [Required]
        public string SmtpHost { get; set; }

        [Range(1, 65535)]
        public int SmtpPort { get; set; }

        public bool UseStartTls { get; set; }

        [Required]
        public string SenderAddress { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
namespace EventManagement.ApplicationCore.Models
{
    public class MailSettings : BaseEntity
    {
        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; } = 25;

        public bool UseStartTls { get; set; }

        public string SenderAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public Event Event { get; set; }
    }
}
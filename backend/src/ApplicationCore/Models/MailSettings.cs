using System.Collections.Generic;

namespace EventManagement.ApplicationCore.Models
{
    public class MailSettings : BaseEntity
    {
        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; } = 25;

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public bool UseStartTls { get; set; }

        public string SenderAddress { get; set; }

        public string Subject { get; set; } = "Your ticket | {{ EventName }}";

        public string Body { get; set; } = @"{{#if FirstName}}Hi {{FirstName}}{{else}}Hey Dude{{/if}},

with this e-mail you're receiving your personal ticket for the {{ EventName }}.

We're looking forward to seeing you!
{{ EventHost }} Team

{{ EventHomepageUrl }}
";

        public bool EnableDemoMode { get; set; }

        public Event Event { get; set; }
        public IList<DemoEmailRecipient> DemoEmailRecipients { get; set; }
    }
}
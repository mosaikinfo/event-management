using System;

namespace EventManagement.ApplicationCore.Models
{
    public class DemoEmailRecipient : BaseEntity
    {
        public DemoEmailRecipient()
        {
        }

        public DemoEmailRecipient(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public Guid MailSettingsId { get; set; }

        public string EmailAddress { get; set; }
    }
}
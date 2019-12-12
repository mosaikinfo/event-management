using System;

namespace EventManagement.ApplicationCore.Models
{
    public class AuditEvent : BaseEntity
    {
        public AuditEvent()
        {
        }

        public AuditEvent(bool succeeded)
        {
            Level = succeeded ? AuditEventLevel.Success : AuditEventLevel.Fail;
        }

        public DateTime Time { get; set; }

        public string Action { get; set; }

        public string Detail { get; set; }

        public AuditEventLevel Level { get; set; } = AuditEventLevel.Success;

        public Guid TicketId { get; set; }

        public Ticket Ticket { get; set; }
    }
}
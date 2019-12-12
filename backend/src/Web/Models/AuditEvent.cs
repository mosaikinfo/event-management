using System;

namespace EventManagement.WebApp.Models
{
    public class AuditEvent
    {
        public DateTime Time { get; set; }

        public string Action { get; set; }

        public string Detail { get; set; }

        public string Level { get; set; }
    }
}
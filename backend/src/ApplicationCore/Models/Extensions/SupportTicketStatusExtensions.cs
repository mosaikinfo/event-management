using System;

namespace EventManagement.ApplicationCore.Models.Extensions
{
    public static class SupportTicketStatusExtensions
    {
        public static string GetStringValue(this SupportTicketStatus value)
        {
            return value.ToString().ToLowerInvariant();
        }

        public static SupportTicketStatus FromStringValue(string value)
        {
            return (SupportTicketStatus)Enum.Parse(typeof(SupportTicketStatus), value, true);
        }
    }
}
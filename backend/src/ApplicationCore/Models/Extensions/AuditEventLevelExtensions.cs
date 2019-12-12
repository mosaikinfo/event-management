namespace EventManagement.ApplicationCore.Models.Extensions
{
    public static class AuditEventLevelExtensions
    {
        public static string GetStringValue(this AuditEventLevel level)
        {
            return level.ToString().ToLowerInvariant();
        }
    }
}

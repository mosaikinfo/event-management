namespace EventManagement.ApplicationCore.Models
{
    public static class UserRoleExtensions
    {
        public static string GetStringValue(this UserRole role)
        {
            return role.ToString().ToLowerInvariant();
        }
    }
}
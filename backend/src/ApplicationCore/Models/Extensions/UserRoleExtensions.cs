namespace EventManagement.ApplicationCore.Models.Extensions
{
    public static class UserRoleExtensions
    {
        public static string GetStringValue(this UserRole role)
        {
            return role.ToString().ToLowerInvariant();
        }
    }
}
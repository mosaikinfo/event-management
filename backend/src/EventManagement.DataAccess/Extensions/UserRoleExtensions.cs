using EventManagement.DataAccess.Models;

namespace EventManagement.DataAccess.Extensions
{
    public static class UserRoleExtensions
    {
        public static string GetStringValue(this UserRole role)
        {
            return role.ToString().ToLowerInvariant();
        }
    }
}

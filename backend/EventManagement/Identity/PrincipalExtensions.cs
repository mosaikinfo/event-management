using IdentityServer4.Extensions;
using System.Security.Principal;

namespace EventManagement.Identity
{
    public static class PrincipalExtensions
    {
        /// <summary>
        /// Get the current user id.
        /// </summary>
        public static int GetUserId(this IPrincipal principal)
        {
            int userId;
            var subject = principal.GetSubjectId();
            int.TryParse(subject, out userId);
            return userId;
        }
    }
}
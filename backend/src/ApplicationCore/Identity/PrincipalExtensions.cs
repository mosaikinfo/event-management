using IdentityServer4.Extensions;
using System;
using System.Security.Principal;

namespace EventManagement.Identity
{
    public static class PrincipalExtensions
    {
        /// <summary>
        /// Get the current user id.
        /// </summary>
        public static Guid GetUserId(this IPrincipal principal)
        {
            var subject = principal.GetSubjectId();
            Guid.TryParse(subject, out Guid userId);
            return userId;
        }
    }
}
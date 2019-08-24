using IdentityModel;
using IdentityServer4.Extensions;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace EventManagement.Identity
{
    public static class PrincipalExtensions
    {
        /// <summary>
        /// Returns true, if the authenticated identity is a real person.
        /// Returns false, if it is only an API Client (S2S App) without user context.
        /// </summary>
        /// <param name="principal">The authenticated user</param>
        public static bool IsPerson(this IPrincipal principal)
        {
            var id = principal.Identity as ClaimsIdentity;
            return id.FindFirst(JwtClaimTypes.Subject) != null;
        }

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
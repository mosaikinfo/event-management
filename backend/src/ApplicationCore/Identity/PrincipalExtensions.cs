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
        /// Get information about the context in which the current user is currently working.
        /// </summary>
        /// <returns></returns>
        public static UserContext GetContext(this IPrincipal principal)
        {
            if (!principal.IsAuthenticated())
                return null;

            return new UserContext
            {
                UserId = principal.GetUserId(),
                EventId = principal.GetEventId()
            };
        }

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

        /// <summary>
        /// Get the id of the event for which the user is currently working.
        /// </summary>
        public static Guid? GetEventId(this IPrincipal principal)
        {
            var id = principal.Identity as ClaimsIdentity;
            string eventIdStringValue = id.FindFirst(EventManagementClaimTypes.EventId)?.Value;
            if (eventIdStringValue != null)
            {
                Guid.TryParse(eventIdStringValue, out Guid eventId);
                return eventId == Guid.Empty ? null : (Guid?) eventId;
            }
            return null;
        }

    }
}
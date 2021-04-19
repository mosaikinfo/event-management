using EventManagement.WebApp.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Shared.Mvc
{
    public class EventManagementController : Controller
    {
        /// <summary>
        /// Get the URI of the Ticket Validation Endpoint.
        /// The URI contains an placeholder that should be replaced with the ticket secret.
        /// Example: http://myevent/validate/--secret---
        /// </summary>
        protected string GetTicketValidationUriFormatString()
        {
            return GetTicketValidationUri(
                EventManagementConstants.TicketGeneration.SecretUrlPlaceholder);
        }

        /// <summary>
        /// Build the URI to validate a ticket at entrance control.
        /// </summary>
        protected string GetTicketValidationUri(string secret)
        {
            return Url.RouteAbsoluteUrl(
                TicketValidationController.TicketValidationRouteName,
                new { secret = secret });
        }

        /// <summary>
        /// Tries to resolve the authenticated user by using multiple authentication schemes in this order:
        /// 
        /// 1. The user is an event admin authenticated to the backend.
        /// 2. It is an API client authenticated with an access token.
        /// 3. Master QR Code (login for entrance control).
        /// </summary>
        protected async Task<ClaimsPrincipal> TryGetAuthenticatedUser()
        {
            // check default auth cookie.
            if (!User.Identity.IsAuthenticated)
            {
                var auth = await HttpContext.AuthenticateAsync(
                    EventManagementConstants.MasterQrCode.AuthenticationScheme);
                // check master qr auth cookie.
                if (auth.Succeeded)
                {
                    return auth.Principal;
                }
            }
            return User;
        }
    }
}
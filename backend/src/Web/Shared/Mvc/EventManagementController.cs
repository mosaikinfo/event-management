using EventManagement.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

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
            return Url.ActionAbsoluteUrl<TicketValidationController>(
                nameof(TicketValidationController.ValidateTicketByQrCodeValueAsync),
                new { secret = secret });
        }
    }
}
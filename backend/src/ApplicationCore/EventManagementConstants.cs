namespace EventManagement
{
    public static class EventManagementConstants
    {
        public static class AdminApi
        {
            /// <summary>
            /// The API scope name that gives access to the Event Management API.
            /// </summary>
            public const string ScopeName = "eventmanagement:admin";

            /// <summary>
            /// Display name for the consent screen.
            /// </summary>
            public const string DisplayName = "Access the Admin API";

            /// <summary>
            /// The name of the authorization policy for the Event Management API.
            /// </summary>
            public const string PolicyName = "EventManagementCookieOrAccessToken";
        }

        public static class MasterQrCode
        {
            /// <summary>
            /// Name of the authentication scheme to authenticate with a
            /// qr code scanner app by scanning a Master QR Code.
            /// </summary>
            public const string AuthenticationScheme = "masterqr";
        }

        public static class TicketGeneration
        {
            public static string SecretUrlPlaceholder = "--secret--";
        }

        public static class Auditing
        {
            public static class Actions
            {
                public const string TicketOrder = "ticket_order";
                public const string EmailSent = "email_sent";
            }
        }
    }
}
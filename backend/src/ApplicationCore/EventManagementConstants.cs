namespace EventManagement
{
    public static class EventManagementConstants
    {
        public static class AdminApi
        {
            /// <summary>
            /// The API scope name that gives access to the Event Management API.
            /// </summary>
            public const string ScopeName = "IdentityServerApi";

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
    }
}
namespace EventManagement.WebApp
{
    public static class Constants
    {
        /// <summary>
        /// Name of the authentication scheme to authenticate with a
        /// qr code scanner app by scanning a Master QR Code.
        /// </summary>
        public const string MasterQrCodeAuthenticationScheme = "masterqr";

        /// <summary>
        /// Policy to authorize users that want to validate tickets with a qr code scanner.
        /// </summary>
        public const string QrScanPolicy = "TicketValidation";
    }
}

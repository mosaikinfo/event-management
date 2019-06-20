namespace EventManagement.ApplicationCore.Models
{
    public static class PaymentStatusExtensions
    {
        public static string GetStringValue(this PaymentStatus paymentStatus)
        {
            return paymentStatus.ToString().ToLowerInvariant();
        }
    }
}
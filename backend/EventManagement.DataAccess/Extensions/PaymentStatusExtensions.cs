using EventManagement.DataAccess.Models;

namespace EventManagement.DataAccess.Extensions
{
    public static class PaymentStatusExtensions
    {
        public static string GetStringValue(this PaymentStatus paymentStatus)
        {
            return paymentStatus.ToString().ToLowerInvariant();
        }
    }
}
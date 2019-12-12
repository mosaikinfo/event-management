namespace EventManagement.ApplicationCore.Models.Extensions
{
    public static class TicketDeliveryTypeExtensions
    {
        public static string GetStringValue(this TicketDeliveryType deliveryType)
        {
            return deliveryType.ToString().ToLowerInvariant();
        }
    }
}
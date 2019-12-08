namespace EventManagement.ApplicationCore.Models.Extensions
{
    public static class PaymentStatusExtensions
    {
        public static string GetDescription(this PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case PaymentStatus.Open:
                    return "offen";

                case PaymentStatus.PaidPartial:
                    return "Teilweise bezahlt";

                case PaymentStatus.Paid:
                    return "bezahlt";
            }
            return null;
        }

        public static string GetStringValue(this PaymentStatus paymentStatus)
        {
            return paymentStatus.ToString().ToLowerInvariant();
        }
    }
}
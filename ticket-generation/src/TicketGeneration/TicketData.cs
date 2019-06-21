using System.Collections.Generic;

namespace EventManagement.TicketGeneration
{
    /// <summary>
    /// Variables to write into the ticket.
    /// </summary>
    public class TicketData
    {
        public string EventName { get; set; }
        public string TicketId { get; set; }
        public string QrValue { get; set; }
        public string EventLogo { get; set; }
        public string Host { get; set; }
        public string EventDate { get; set; }
        public string EventLocation { get; set; }
        public string TicketType { get; set; }
        public string Price { get; set; }
        public string EntranceTime { get; set; }
        public string BeginTime { get; set; }
        public IList<string> Address { get; set; } = new List<string>();
        public string QrTrafficImageUrl { get; set; }
        public string Buyer { get; set; }
        public string BookingDate { get; set; }
        public string BookingNumber { get; set; }
    }
}
namespace EventManagement.WebApp.Models
{
    public class EventFilterParams
    {
        /// <summary>
        /// Filter the events by their location (must contain string).
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Return only events in the future.
        /// </summary>
        public bool? Future { get; set; }

        /// <summary>
        /// Returns only events for which ticket sales have already been started.
        /// </summary>
        public bool? TicketSaleHasStarted { get; set; }
    }
}

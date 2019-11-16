namespace EventManagement.WebApp.Models
{
    public class TicketQuotaReportRow
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public int? Quota { get; set; }

        /// <summary>
        /// Number of existing tickets of this ticket type.
        /// </summary>
        public int Count { get; set; }
    }
}
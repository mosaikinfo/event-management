using System.Collections.Generic;

namespace EventManagement.DataAccess.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Event Event { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
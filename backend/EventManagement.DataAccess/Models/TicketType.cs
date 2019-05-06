using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.DataAccess.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Event Event { get; set; }
    }
}

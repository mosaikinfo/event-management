using System;
using System.Collections.Generic;

namespace EventManagement.ApplicationCore.Models
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? EntranceTime { get; set; }
        public string Location { get; set; }

        public List<TicketType> TicketTypes { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
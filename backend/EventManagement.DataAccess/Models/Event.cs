using System;

namespace EventManagement.DataAccess.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? EntranceTime { get; set; }
    }
}

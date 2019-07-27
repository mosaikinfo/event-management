using System;

namespace EventManagement.WebApp.Models
{
    public class TicketType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
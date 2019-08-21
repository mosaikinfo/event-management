using System;

namespace EventManagement.ApplicationCore.Models
{
    /// <summary>
    /// S2S Client to access the Event Management API.
    /// </summary>
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string Secret { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
    }
}
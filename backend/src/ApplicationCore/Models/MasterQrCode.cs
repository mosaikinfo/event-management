using System;

namespace EventManagement.ApplicationCore.Models
{
    public class MasterQrCode : BaseEntity
    {
        public Guid EventId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public Event Event { get; set; }
        public User Owner { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.WebApp.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Secret { get; set; }

        public bool Enabled { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
    }
}
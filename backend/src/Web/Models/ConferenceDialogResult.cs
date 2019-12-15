using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.WebApp.Models
{
    public class ConferenceDialogResult
    {
        [Required]
        public Guid TicketId { get; set; }
    }
}

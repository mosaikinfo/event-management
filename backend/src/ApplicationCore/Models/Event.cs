using System;
using System.Collections.Generic;

namespace EventManagement.ApplicationCore.Models
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// If the event is a conference this affects nearly every process.
        /// If it's not a conference (eg: a concert) the personal information doesn't matter.
        /// </summary>
        public bool IsConference { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime? EntranceTime { get; set; }

        public string Location { get; set; }

        public string HomepageUrl { get; set; }

        /// <summary>
        /// True, if you want to add personal information in form 
        /// of a JSON Web Token (JWT) when redirecting to the homepage.
        /// </summary>
        public bool IncludePersonalInformation { get; set; }

        public string Host { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public Guid? MailSettingsId { get; set; }

        public bool IsDeleted { get; set; }

        public List<TicketType> TicketTypes { get; set; }

        public List<Ticket> Tickets { get; set; }

        public MailSettings MailSettings { get; set; }
    }
}
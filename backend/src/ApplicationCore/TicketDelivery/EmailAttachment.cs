using System.IO;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }
}
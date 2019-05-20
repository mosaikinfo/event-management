// TODO: We need two-factor authentication for downloading a ticket. A secret url isn't secure enough.
using EventManagement.DataAccess;
using EventManagement.TicketGeneration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to download tickets with an internet browser.
    /// </summary>
    [Route("tickets")]
    public class TicketDownloadController : Controller
    {
        private readonly EventsDbContext _context;

        public TicketDownloadController(EventsDbContext context)
        {
            _context = context;
        }

        [Route("{ticketGuid}")]
        public IActionResult DownloadAsPdf(Guid ticketGuid)
        {
            var ticket = _context.Tickets.SingleOrDefault(x => x.TicketGuid == ticketGuid);
            if (ticket == null)
                return NotFound();
            var ticketData = new TicketData
            {
                EventName = "ONE",
                TicketId = "123780112",
                QrValue = "https://one-movement.de",
                EventLogo = "https://one-movement.de/wp-content/uploads/2018/10/One_Events_2019.png",
                Host = "ONE Network",
                EventDate = "Samstag, 06.04.2019",
                EventLocation = "Neu-Ulm",
                TicketType = "Tageskonferenz + ONE Night",
                Price = "30 € (inkl. Vorverkaufsgebühr)",
                EntranceTime = "18:30 Uhr",
                BeginTime = "19:00 Uhr",
                Address =
                {
                    "ratiopharm arena",
                    "Europastraße 25",
                    "89231 Neu-Ulm"
                },
                QrTrafficImageUrl = "http://placehold.jp/150x150.png",
                Buyer = "John Doe",
                BookingDate = "01.04.2019",
                BookingNumber = "12892984"
            };
            var stream = new MemoryStream();
            var generator = new PdfTicketGenerator();
            generator.GenerateTicket(ticketData, stream);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            string fileDownloadName = ticketData.TicketId + ".pdf";
            return File(stream, "application/pdf", fileDownloadName);
        }
    }
}

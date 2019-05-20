using EventManagement.DataAccess;
using EventManagement.TicketGeneration;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to download tickets with an internet browser.
    /// </summary>
    [SecurityHeaders]
    [Authorize(AuthenticationSchemes = IdentityServerConstants.DefaultCookieAuthenticationScheme)]
    public class TicketDownloadController : Controller
    {
        private readonly EventsDbContext _context;

        public TicketDownloadController(EventsDbContext context)
        {
            _context = context;
        }

        [HttpGet("tickets/{id}/pdf")]
        public IActionResult DownloadAsPdf(int id)
        {
            var ticket = _context.Tickets.Find(id);

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
            stream.Position = 0;

            string fileDownloadName = ticketData.TicketId + ".pdf";
            Response.Headers.Add("Content-Disposition", $"inline; filename={fileDownloadName}");
            return File(stream, "application/pdf");
        }
    }
}
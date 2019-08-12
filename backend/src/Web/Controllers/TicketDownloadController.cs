using EventManagement.Infrastructure.Data;
using EventManagement.TicketGeneration;
using EventManagement.WebApp.Shared.Mvc;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

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
        public IActionResult DownloadAsPdf(Guid id)
        {
            var ticket = _context.Tickets
                .Include(x => x.Event)
                .Include(x => x.TicketType)
                .SingleOrDefault(x => x.Id == id);

            if (ticket == null)
                return NotFound();

            TicketData values = Map(ticket);

            var stream = new MemoryStream();
            var generator = new PdfTicketGenerator();
            generator.GenerateTicket(values, stream);
            stream.Position = 0;

            string fileDownloadName = ticket.TicketNumber + ".pdf";
            Response.Headers.Add("Content-Disposition", $"inline; filename={fileDownloadName}");
            return File(stream, "application/pdf");
        }

        private TicketData Map(ApplicationCore.Models.Ticket ticket)
        {
            var values = new TicketData
            {
                EventName = ticket.Event.Name,
                TicketId = ticket.TicketNumber,
                QrValue = GetTicketValidationUrl(ticket),
                EventLogo = "https://one-movement.de/wp-content/uploads/2018/10/One_Events_2019.png",
                Host = "ONE Network",
                EventDate = ticket.Event.StartTime.ToString("dddd, dd.MM.yyyy"),
                EventLocation = ticket.Event.Location,
                TicketType = ticket.TicketType.Name,
                Price = $"{ticket.TicketType.Price} € (inkl. Vorverkaufsgebühr)",
                BeginTime = ticket.Event.StartTime.ToString("hh:mm") + " Uhr",
                Address =
                    ticket.Event.Location
                        .Split(',').Select(s => s.Trim())
                        .Where(s => s.Length > 0).ToList(),
                BookingDate = ticket.CreatedAt.ToString("dd.MM.yyyy"),
                BookingNumber = ticket.TicketNumber
            };
            if (ticket.LastName != null)
            {
                values.Buyer = $"{ticket.FirstName} {ticket.LastName}".TrimStart();
            }
            if (ticket.Event.EntranceTime != null)
            {
                values.EntranceTime =
                    ticket.Event.EntranceTime.Value.ToString("hh:mm") + " Uhr";
            }
            return values;
        }

        private string GetTicketValidationUrl(ApplicationCore.Models.Ticket ticket)
        {
            return Url.ActionAbsoluteUrl<TicketValidationController>(
                nameof(TicketValidationController.ValidateTicketAsync),
                new { secret = ticket.TicketSecret });
        }
    }
}
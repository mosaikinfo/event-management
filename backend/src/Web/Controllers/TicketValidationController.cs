using EventManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace EventManagement.WebApp.Controllers
{
    public class TicketValidationController : Controller
    {
        private readonly EventsDbContext _context;
        private readonly ILogger _logger;

        public TicketValidationController(EventsDbContext context,
                                          ILogger<TicketValidationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("v/{secret}")]
        public IActionResult ValidateTicket(string secret)
        {
            ApplicationCore.Models.Ticket ticket =
                _context.Tickets
                    .Include(e => e.Event)
                    .Include(e => e.TicketType)
                    .SingleOrDefault(e => e.TicketSecret == secret);
            if (ticket == null)
            {
                _logger.LogInformation("Ticket with secret {id} was not found in the database.", secret);
                return TicketNotFound();
            }
            if (ticket.Validated)
            {
                _logger.LogInformation("The ticket has been already used before.");
                return View("TicketUsed", ticket);
            }
            ticket.Validated = true;
            _context.SaveChanges();
            return View("TicketValid", ticket);
        }

        private IActionResult TicketNotFound()
        {
            ViewBag.ErrorMessage = "Dieses Ticket existiert leider nicht!";
            return View("TicketError");
        }
    }
}
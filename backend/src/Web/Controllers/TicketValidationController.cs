using EventManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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

        [HttpGet("v/{id}")]
        public IActionResult ValidateTicket(string id)
        {
            Guid ticketGuid;
            if (!Guid.TryParse(id, out ticketGuid))
            {
                _logger.LogInformation("The parameter id is no valid guid.");
                return TicketNotFound();
            }
            ApplicationCore.Models.Ticket ticket =
                _context.Tickets
                    .Include(e => e.Event)
                    .Include(e => e.TicketType)
                    .SingleOrDefault(e => e.TicketGuid == ticketGuid);
            if (ticket == null)
            {
                _logger.LogInformation("Ticket with id {id} was not found in the database.", ticketGuid);
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
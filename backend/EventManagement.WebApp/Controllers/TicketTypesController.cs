using EventManagement.DataAccess;
using EventManagement.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = Constants.JwtAuthScheme)]
    public class TicketTypesController : ControllerBase
    {
        private readonly EventsDbContext _context;

        public TicketTypesController(EventsDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/event/{eventId}/tickettypes")]
        public ActionResult<IList<TicketType>> GetTicketTypes(int eventId)
        {
            throw new NotImplementedException();
        }
    }
}

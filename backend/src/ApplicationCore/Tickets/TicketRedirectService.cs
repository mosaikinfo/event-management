using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.Identity;
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Models.Extensions;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Tickets
{
    public class TicketRedirectService : ITicketRedirectService
    {
        private ITicketsRepository _tickets;
        private IJwtTokenService _tokenService;

        public TicketRedirectService(ITicketsRepository tickets, IJwtTokenService tokenService)
        {
            _tickets = tickets;
            _tokenService = tokenService;
        }

        public async Task<string> GetRedirectUrlAsync(Guid ticketId, string ticketValidationUrl)
        {
            Ticket ticket = await _tickets.GetAsync(ticketId);

            if (ticket == null)
                throw new TicketNotFoundException();

            var claims = GetClaims(ticket, ticketValidationUrl);
            int lifetime = 365 * 24 * 3600; // 365 days.
            var jwt = await _tokenService.IssueJwtAsync(lifetime, claims);
            var sb = new StringBuilder(ticket.Event.HomepageUrl);
            sb.Append(jwt);
            return sb.ToString();
        }

        private IEnumerable<Claim> GetClaims(Ticket ticket, string ticketValidationUrl)
        {
            if (ticket.FirstName != null)
                yield return new Claim(JwtClaimTypes.GivenName, ticket.FirstName);
            if (ticket.LastName != null)
                yield return new Claim(JwtClaimTypes.FamilyName, ticket.LastName);

            if (ticket.BirthDate != null)
            {
                string birthdate = ticket.BirthDate.Value.ToString("yyyy-MM-dd");
                yield return new Claim(JwtClaimTypes.BirthDate, birthdate);
            }

            if (ticket.Gender.HasValue)
                yield return new Claim(JwtClaimTypes.Gender, ticket.Gender.Value.GetStringValue());

            if (ticket.RoomNumber != null)
                yield return new Claim(EventManagementClaimTypes.Room, ticket.RoomNumber);

            yield return new Claim(EventManagementClaimTypes.EventId, ticket.EventId.ToString());

            if (ticketValidationUrl != null)
                yield return new Claim(EventManagementClaimTypes.QrCode, ticketValidationUrl);

            yield return new Claim(EventManagementClaimTypes.TicketType, ticket.TicketType.Name);
        }
    }
}
using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
using IdentityModel;
using System.Text;

namespace EventManagement.ApplicationCore.Services
{
    public class TicketNumberService : ITicketNumberService
    {
        public string GenerateTicketNumber(Event evt)
        {
            var random = new CryptoRandom();
            var sb = new StringBuilder();
            sb.Append(evt.Id.ToString().PadRight(3, '0'));
            for (int i = 0; i < 7; i++)
            {
                sb.Append(random.Next(10));
            }
            return sb.ToString();
        }
    }
}
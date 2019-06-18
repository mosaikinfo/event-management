using IdentityModel;
using System.Text;

namespace EventManagement
{
    public static class TicketNumberHelper
    {
        /// <summary>
        /// Generate a random human-readable ticket number.
        /// </summary>
        /// <param name="evt">Event for the ticket.</param>
        public static string GenerateTicketNumber(DataAccess.Models.Event evt)
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
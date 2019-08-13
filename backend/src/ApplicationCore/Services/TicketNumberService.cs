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
            var sb = new StringBuilder();

            // Sum of numbers of the year. Example: 2019 => 2 + 1 + 9 = 12
            string year = SumDigits(evt.StartTime.Year)
                .ToString().PadRight(2, '0').Substring(0, 2);
            sb.Append(year);

            // Length of the event name.
            sb.Append(evt.Name.Length % 10);

            var random = new CryptoRandom();
            for (int i = 0; i < 7; i++)
            {
                sb.Append(random.Next(10));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Builds the sum of each single digit of the number.
        /// </summary>
        private static int SumDigits(int number)
        {
            int sum = 0;
            while (number != 0)
            {
                sum += number % 10;
                number /= 10;
            }
            return sum;
        }
    }
}
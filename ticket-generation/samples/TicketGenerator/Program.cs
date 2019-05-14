using EventManagement.TicketGeneration;
using System;
using System.Diagnostics;

namespace TicketGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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

            var generator = new PdfTicketGenerator();
            string filePath = generator.GenerateTicket(ticketData);

            Console.WriteLine("Ticket was saved as file:\n{0}", filePath);
            // Open in pdf viewer.
            Process.Start("cmd.exe", $"/c {filePath}");
        }
    }
}
﻿using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Tickets;
using EventManagement.TicketGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketGeneration
{
    public class PdfTicketService : IPdfTicketService
    {
        private readonly ITicketsRepository _tickets;

        public PdfTicketService(ITicketsRepository tickets)
        {
            _tickets = tickets;
        }

        public async Task<Stream> GeneratePdfAsync(Guid ticketId, string ticketValidationUriFormat)
        {
            var ticket = await _tickets.GetAsync(ticketId);

            if (ticket == null)
                throw new TicketNotFoundException();

            TicketData ticketData = Map(ticket, ticketValidationUriFormat);

            var stream = new MemoryStream();
            var generator = new PdfTicketGenerator();
            generator.GenerateTicket(ticketData, stream);
            stream.Position = 0;
            return stream;
        }

        private TicketData Map(Ticket ticket, string ticketValidationUriFormat)
        {
            var validationUri = ticketValidationUriFormat.Replace(
                EventManagementConstants.TicketGeneration.SecretUrlPlaceholder,
                ticket.TicketSecret);

            // TODO: configure logo in event settings.
            string logoUrl = new Uri(new Uri(validationUri), "/mosaik.png").AbsoluteUri;

            // TODO: configure timezone in event settings.
            // (UTC+01:00) Amsterdam, Berlin, Bern, Rom, Stockholm, Wien
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(ticket.Event.StartTime, timezone);

            // TODO: make date time format configurable in event settings.
            var values = new TicketData
            {
                EventName = ticket.Event.Name,
                EventLogo = logoUrl,
                TicketId = ticket.TicketNumber,
                QrValue = validationUri,
                Host = ticket.Event.Host,
                EventDate = startTime.ToString("dddd, dd.MM.yyyy"),
                EventLocation = ticket.Event.Location,
                TicketType = ticket.TicketType.Name,
                Transmissible = "false",
                BeginTime = startTime.ToString("HH:mm") + " Uhr",
                Address = GetAddressRows(ticket).ToList(),
                BookingDate = ticket.BookingDate?.ToString("dd.MM.yyyy"),
            };
            if (ticket.LastName != null)
            {
                values.Buyer = $"{ticket.FirstName} {ticket.LastName}".TrimStart();
            }
            if (ticket.Event.EntranceTime != null)
            {
                DateTime entranceTime = TimeZoneInfo.ConvertTimeFromUtc(
                    ticket.Event.EntranceTime.Value, timezone);
                values.EntranceTime = entranceTime.ToString("HH:mm") + " Uhr";
            }
            return values;
        }

        private static IEnumerable<string> GetAddressRows(Ticket ticket)
        {
            foreach (string row in ticket.Event.Address.Split("\n"))
            {
                string s = row.Trim();
                if (s.Length > 0)
                    yield return s;
            }
            yield return $"{ticket.Event.ZipCode} {ticket.Event.City}";
        }
    }
}
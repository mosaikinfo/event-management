using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Services;
using FluentAssertions;
using System;
using Xunit;

namespace EventManagement.UnitTests
{
    public class TicketNumberHelperTests
    {
        [Theory]
        [InlineData("Silvesterkonferenz", 2019, "128")]
        [InlineData("Silvesterkonferenz", 2021, "508")]
        [InlineData("Wacken", 2021, "506")]
        public void GenerateTicketNumber(string eventName, int year, string expectedStart)
        {
            var evt = new Event
            {
                Name = eventName,
                StartTime = new DateTime(year, 1, 1)
            };
            var ticketNumberService = new TicketNumberService();
            string ticketNr = ticketNumberService.GenerateTicketNumber(evt);
            ticketNr.Should().NotBeNull();
            ticketNr.Should().HaveLength(10);
            ticketNr.Should().StartWith(expectedStart);
        }
    }
}
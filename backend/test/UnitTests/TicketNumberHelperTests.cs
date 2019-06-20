using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Services;
using FluentAssertions;
using Xunit;

namespace EventManagement.UnitTests
{
    public class TicketNumberHelperTests
    {
        [Fact]
        public void GenerateTicketNumber()
        {
            var evt = new Event { Id = 1 };
            var ticketNumberService = new TicketNumberService();
            string ticketNr = ticketNumberService.GenerateTicketNumber(evt);
            ticketNr.Should().NotBeNull();
            ticketNr.Should().HaveLength(10);
            // should begin with event id padded right with zeros.
            ticketNr.Should().StartWith("100");
        }
    }
}
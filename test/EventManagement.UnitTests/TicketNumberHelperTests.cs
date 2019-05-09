using EventManagement.DataAccess.Models;
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
            string ticketNr = TicketNumberHelper.GenerateTicketNumber(evt);
            ticketNr.Should().NotBeNull();
            ticketNr.Should().HaveLength(10);
            // should begin with event id padded right with zeros.
            ticketNr.Should().StartWith("100");
        }
    }
}
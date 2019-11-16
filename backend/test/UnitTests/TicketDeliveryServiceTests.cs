using EventManagement.ApplicationCore.Auditing;
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketDelivery;
using EventManagement.ApplicationCore.TicketGeneration;
using EventManagement.ApplicationCore.Tickets;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EventManagement.UnitTests
{
    public class TicketDeliveryServiceTests
    {
        [Fact]
        public async Task SendTicket_Mail()
        {
            var ticketId = Guid.NewGuid();
            var deliveryType = TicketDeliveryType.Email;
            const string validationUri = "http://myevent/";
            const string homePageUrl = "http://myevent/";

            var tickets = new Mock<ITicketsRepository>();
            tickets.Setup(c => c.ExistsAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            var ticketsDeliveryData = new Mock<ITicketDeliveryDataRepository>();
            ticketsDeliveryData.Setup(c => c.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                    new TicketDeliveryData
                    {
                        MailSettings = new MailSettings(),
                        Ticket = new Ticket
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            Mail = "john.doe@itsnotabug.de",
                            TicketType = new TicketType
                            {
                                Name = "VIP",
                                Price = 15.00m
                            },
                            Event = new Event
                            {
                                Name = "ONE Planetshakers Ulm",
                                Location = "Ratiopharm Arena",
                                HomepageUrl = "https://one-movement.de",
                                Host = "ONE Team"
                            }
                        }
                    }));

            var emailService = new Mock<IEmailService>();
            emailService.Setup(c => c.SendMailAsync(It.IsAny<MailSettings>(), It.IsAny<EmailMessage>()));

            var pdfTicketService = new Mock<IPdfTicketService>();

            var auditEventLog = new Mock<IAuditEventLog>();
            auditEventLog.Setup(x => x.AddAsync(It.IsAny<AuditEvent>()));

            var service = new TicketDeliveryService(
                tickets.Object, ticketsDeliveryData.Object,
                emailService.Object, pdfTicketService.Object,
                auditEventLog.Object, Logger);

            await service.SendTicketAsync(ticketId, deliveryType, validationUri, homePageUrl);

            Mock.VerifyAll(tickets, ticketsDeliveryData, emailService, pdfTicketService, auditEventLog);
        }

        [Fact]
        public void SendTicket_NotSupportedDeliveryType()
        {
            var ticketId = Guid.NewGuid();
            var deliveryType = TicketDeliveryType.LetterPost;
            const string validationUri = "http://myevent/";
            const string homePageUrl = "http://myevent/";

            var tickets = new Mock<ITicketsRepository>();
            var ticketsDeliveryData = new Mock<ITicketDeliveryDataRepository>();
            var emailService = new Mock<IEmailService>();
            var pdfTicketService = new Mock<IPdfTicketService>();
            var auditEventLog = new Mock<IAuditEventLog>();
            var service = new TicketDeliveryService(
                tickets.Object, ticketsDeliveryData.Object,
                emailService.Object, pdfTicketService.Object,
                auditEventLog.Object, Logger);

            Func<Task> f = async () => await service
                .SendTicketAsync(ticketId, deliveryType, validationUri, homePageUrl);

            f.Should().Throw<NotSupportedException>();
        }

        private static ILogger<TicketDeliveryService> Logger =>
            new Mock<ILogger<TicketDeliveryService>>().Object;
    }
}
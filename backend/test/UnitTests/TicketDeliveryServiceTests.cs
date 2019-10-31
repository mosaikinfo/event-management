﻿using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketDelivery;
using EventManagement.ApplicationCore.TicketGeneration;
using EventManagement.ApplicationCore.Tickets;
using FluentAssertions;
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

            var tickets = new Mock<ITicketsRepository>();
            tickets.Setup(c => c.ExistsAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            var ticketsDeliveryData = new Mock<ITicketDeliveryDataRepository>();
            ticketsDeliveryData.Setup(c => c.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                    new TicketDeliveryData
                    {
                        MailSettings = new MailSettings(),
                        Ticket = new Ticket()
                    }));

            var emailService = new Mock<IEmailService>();
            emailService.Setup(c => c.SendMailAsync(It.IsAny<MailSettings>(), It.IsAny<EmailMessage>()));

            var pdfTicketService = new Mock<IPdfTicketService>();

            var service = new TicketDeliveryService(
                tickets.Object, ticketsDeliveryData.Object,
                emailService.Object, pdfTicketService.Object);

            await service.SendTicketAsync(ticketId, deliveryType, validationUri);

            Mock.VerifyAll(tickets, ticketsDeliveryData, emailService, pdfTicketService);
        }

        [Fact]
        public void SendTicket_NotSupportedDeliveryType()
        {
            var ticketId = Guid.NewGuid();
            var deliveryType = TicketDeliveryType.LetterPost;
            const string validationUri = "http://myevent/";

            var tickets = new Mock<ITicketsRepository>();
            var ticketsDeliveryData = new Mock<ITicketDeliveryDataRepository>();
            var emailService = new Mock<IEmailService>();
            var pdfTicketService = new Mock<IPdfTicketService>();
            var service = new TicketDeliveryService(
                tickets.Object, ticketsDeliveryData.Object,
                emailService.Object, pdfTicketService.Object);

            Func<Task> f = async () => await service
                .SendTicketAsync(ticketId, deliveryType, validationUri);

            f.Should().Throw<NotSupportedException>();
        }
    }
}
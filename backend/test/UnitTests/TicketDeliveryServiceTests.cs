using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Services;
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

            var repo = new Mock<ITicketDeliveryDataRepository>();
            repo.Setup(c => c.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                    new TicketDeliveryData
                    {
                        MailSettings = new MailSettings(),
                        Ticket = new Ticket()
                    }));

            var emailService = new Mock<IEmailService>();
            emailService.Setup(c => c.SendMailAsync(It.IsAny<MailSettings>(), It.IsAny<EmailMessage>()));

            var service = new TicketDeliveryService(
                repo.Object, emailService.Object);

            await service.SendTicketAsync(ticketId, deliveryType);

            Mock.VerifyAll(repo, emailService);
        }

        [Fact]
        public void SendTicket_NotSupportedDeliveryType()
        {
            var ticketId = Guid.NewGuid();
            var deliveryType = TicketDeliveryType.LetterPost;
            var repo = new Mock<ITicketDeliveryDataRepository>();
            var emailService = new Mock<IEmailService>();
            var service = new TicketDeliveryService(repo.Object, emailService.Object);

            Func<Task> f = async () => await service.SendTicketAsync(ticketId, deliveryType);

            f.Should().Throw<NotSupportedException>();
        }
    }
}

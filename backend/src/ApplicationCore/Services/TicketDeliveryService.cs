using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.Validation;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Services
{
    public class TicketDeliveryService : ITicketDeliveryService
    {
        private readonly ITicketDeliveryDataRepository _ticketDataRepo;
        private readonly IEmailService _emailService;

        public TicketDeliveryService(ITicketDeliveryDataRepository ticketDataRepo,
                                     IEmailService emailService)
        {
            _ticketDataRepo = ticketDataRepo;
            _emailService = emailService;
        }

        public async Task ValidateAsync(Guid ticketId, TicketDeliveryType deliveryType)
        {
            if (deliveryType != TicketDeliveryType.Email)
                throw new NotSupportedException(
                            $"The delivery type {deliveryType} is not yet supported!");

            if (!await _ticketDataRepo.Exists(ticketId))
                throw new TicketNotFoundException();
        }

        [DisplayName("Send {1}")]
        public async Task SendTicketAsync(Guid ticketId, TicketDeliveryType deliveryType)
        {
            await ValidateAsync(ticketId, deliveryType);
            TicketDeliveryData ticketData = await _ticketDataRepo.GetAsync(ticketId);
            await SendTicketByMailAsync(ticketData);
        }

        private Task SendTicketByMailAsync(TicketDeliveryData args)
        {
            if (args.MailSettings == null)
            {
                throw new EventManagementException(
                    "The mail settings haven't been configured for the event.");
            }
            ModelValidator.Validate(args.MailSettings);

            var mail = new EmailMessage
            {
                From = { args.MailSettings.SenderAddress },
                To = { args.Ticket.Mail },
                Subject = args.MailSettings.Subject,
                Body = args.MailSettings.Body
            };

            return _emailService.SendMailAsync(args.MailSettings, mail);
        }
    }
}

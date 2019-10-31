using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketGeneration;
using EventManagement.ApplicationCore.Tickets;
using EventManagement.ApplicationCore.Validation;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    public class TicketDeliveryService : ITicketDeliveryService
    {
        private readonly ITicketsRepository _ticketsRepo;
        private readonly ITicketDeliveryDataRepository _ticketDataRepo;
        private readonly IEmailService _emailService;
        private readonly IPdfTicketService _pdfTicketService;

        public TicketDeliveryService(ITicketsRepository ticketsRepo,
                                     ITicketDeliveryDataRepository ticketDataRepo,
                                     IEmailService emailService,
                                     IPdfTicketService pdfTicketService)
        {
            _ticketsRepo = ticketsRepo;
            _ticketDataRepo = ticketDataRepo;
            _emailService = emailService;
            _pdfTicketService = pdfTicketService;
        }

        public async Task ValidateAsync(Guid ticketId, TicketDeliveryType deliveryType)
        {
            if (deliveryType != TicketDeliveryType.Email)
                throw new NotSupportedException(
                            $"The delivery type {deliveryType} is not yet supported!");

            if (!await _ticketsRepo.ExistsAsync(ticketId))
                throw new TicketNotFoundException();
        }

        [DisplayName("Send {1}")]
        public async Task SendTicketAsync(Guid ticketId, TicketDeliveryType deliveryType, string ticketValidationUriFormat)
        {
            await ValidateAsync(ticketId, deliveryType);
            TicketDeliveryData ticketData = await _ticketDataRepo.GetAsync(ticketId);
            await SendTicketByMailAsync(ticketData, ticketValidationUriFormat);
        }

        private async Task SendTicketByMailAsync(TicketDeliveryData args, string ticketValidationUriFormat)
        {
            if (args.MailSettings == null)
            {
                throw new EventManagementException(
                    "The mail settings haven't been configured for the event.");
            }
            ModelValidator.Validate(args.MailSettings);

            System.IO.Stream stream = await _pdfTicketService
                .GeneratePdfAsync(args.Ticket.Id, ticketValidationUriFormat);

            var mail = new EmailMessage
            {
                From = { args.MailSettings.SenderAddress },
                To = { args.Ticket.Mail },
                Subject = args.MailSettings.Subject,
                Body = args.MailSettings.Body,
                Attachments =
                {
                    new EmailAttachment
                    {
                        FileName = "ticket.pdf",
                        ContentType = "application/pdf",
                        Stream = stream
                    }
                }
            };

            await _emailService.SendMailAsync(args.MailSettings, mail);
        }
    }
}
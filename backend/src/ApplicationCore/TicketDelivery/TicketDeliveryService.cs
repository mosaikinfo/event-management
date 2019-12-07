using EventManagement.ApplicationCore.Auditing;
using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.Models;
using EventManagement.ApplicationCore.TicketGeneration;
using EventManagement.ApplicationCore.Tickets;
using EventManagement.ApplicationCore.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    public class TicketDeliveryService : ITicketDeliveryService
    {
        private readonly ITicketsRepository _ticketsRepo;
        private readonly ITicketDeliveryDataRepository _ticketDataRepo;
        private readonly IEmailService _emailService;
        private readonly IPdfTicketService _pdfTicketService;
        private readonly IAuditEventLog _auditEventLog;
        private readonly ILogger _logger;

        public TicketDeliveryService(ITicketsRepository ticketsRepo,
                                     ITicketDeliveryDataRepository ticketDataRepo,
                                     IEmailService emailService,
                                     IPdfTicketService pdfTicketService,
                                     IAuditEventLog auditEventLog,
                                     ILogger<TicketDeliveryService> logger)
        {
            _ticketsRepo = ticketsRepo;
            _ticketDataRepo = ticketDataRepo;
            _emailService = emailService;
            _pdfTicketService = pdfTicketService;
            _auditEventLog = auditEventLog;
            _logger = logger;
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
        public async Task SendTicketAsync(Guid ticketId,
                                          TicketDeliveryType deliveryType,
                                          string ticketValidationUriFormat,
                                          string homepageUrl)
        {
            await ValidateAsync(ticketId, deliveryType);
            TicketDeliveryData ticketData = await _ticketDataRepo.GetAsync(ticketId);
            await SendTicketByMailAsync(ticketData, ticketValidationUriFormat, homepageUrl);
        }

        private async Task SendTicketByMailAsync(TicketDeliveryData args,
                                                 string ticketValidationUriFormat,
                                                 string homepageUrl)
        {
            if (args.MailSettings == null)
            {
                throw new EventManagementException(
                    "The mail settings haven't been configured for the event.");
            }
            ModelValidator.Validate(args.MailSettings);
            if (string.IsNullOrEmpty(args.Ticket.Mail))
            {
                await LogAuditEvent(args, false, "Die E-Mail-Adresse des Empfängers fehlt.");
                throw new EventManagementException("The ticket has no email address.");
            }

            System.IO.Stream stream = await _pdfTicketService
                .GeneratePdfAsync(args.Ticket.Id, ticketValidationUriFormat);

            IList<string> recipients = new[] { args.Ticket.Mail };

            if (args.MailSettings.EnableDemoMode)
            {
                if (args.MailSettings.DemoEmailRecipients.Any())
                {
                    _logger.LogWarning("Demo Mode is enabled. The e-mails will be sent to a predefined list of recipients only.");
                    recipients = args.MailSettings.DemoEmailRecipients
                        .Select(r => r.EmailAddress)
                        .ToList();
                }
                else
                {
                    _logger.LogWarning("Demo Mode is enabled, but no test recipients are defined.");
                }
            }

            var message = new EmailMessage
            {
                From = { args.MailSettings.SenderAddress },
                To = recipients,
                Subject = args.MailSettings.Subject,
                Body = args.MailSettings.Body,
                Attachments =
                    {
                        new EmailAttachment
                        {
                            FileName = $"ticket-{args.Ticket.TicketNumber}.pdf",
                            ContentType = "application/pdf",
                            Stream = stream
                        }
                    }
            };

            if (!string.IsNullOrEmpty(args.MailSettings.ReplyToAddress))
                message.ReplyTo.Add(args.MailSettings.ReplyToAddress);

            var mail = await EmailTemplateService
                .RenderTicketMailAsync(message, args.Ticket, homepageUrl);

            try
            {
                await _emailService.SendMailAsync(args.MailSettings, mail);
            }
            catch
            {
                await LogAuditEvent(args, false);
                throw;
            }

            await _ticketDataRepo.UpdateDeliveryStatusAsync(args.Ticket.Id,
                true, DateTime.UtcNow, TicketDeliveryType.Email);
            await LogAuditEvent(args, true);
        }

        private Task LogAuditEvent(TicketDeliveryData args, bool succeeded, string detail = null)
        {
            if (detail == null)
            {
                detail = succeeded
                    ? $"Ticket wurde per E-Mail zugestellt an {args.Ticket.Mail}"
                    : $"Die E-Mail konnte leider nicht an {args.Ticket.Mail} zugestellt werden.";
            }
            return _auditEventLog.AddAsync(new AuditEvent
            {
                Time = DateTime.UtcNow,
                TicketId = args.Ticket.Id,
                Action = EventManagementConstants.Auditing.Actions.EmailSent,
                Detail = detail,
                Succeeded = succeeded
            });
        }
    }
}
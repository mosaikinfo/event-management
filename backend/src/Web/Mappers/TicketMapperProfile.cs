using AutoMapper;
using EventManagement.ApplicationCore.Models;
using EventManagement.Infrastructure.Data;
using System;

namespace EventManagement.WebApp.Mappers
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<Ticket, Models.Ticket>()
                .ForMember(e => e.Creator, opt => opt.MapFrom(e => e.Creator.Name))
                .ForMember(e => e.Editor, opt => opt.MapFrom(e => e.Editor.Name))
                .ForMember(e => e.Gender, opt => opt.MapFrom(
                    e => e.Gender.HasValue ? e.Gender.Value.GetStringValue() : null))
                .ReverseMap()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.TicketSecret, opt => opt.Ignore())
                .ForMember(e => e.Creator, opt => opt.Ignore())
                .ForMember(e => e.Editor, opt => opt.Ignore())
                .ForMember(e => e.Gender, opt => opt.MapFrom(
                    e => GenderExtensions.FromStringValue(e.Gender)))
                .ForMember(e => e.BirthDate, opt => opt.MapFrom(
                    e => e.BirthDate.HasValue ? (DateTime?)e.BirthDate.Value.ToLocalTime().Date : null))
                .ForMember(e => e.PaymentStatus, opt => opt.MapFrom<PaymentStatusResolver>());
        }

        public class PaymentStatusResolver : IValueResolver<Models.Ticket, Ticket, PaymentStatus>
        {
            private readonly EventsDbContext _context;

            public PaymentStatusResolver(EventsDbContext context)
            {
                _context = context;
            }

            public PaymentStatus Resolve(Models.Ticket source, Ticket destination,
                                         PaymentStatus destMember, ResolutionContext context)
            {
                if (source.AmountPaid != destination.AmountPaid)
                {
                    if (source.AmountPaid == null || source.AmountPaid == 0.0)
                    {
                        return PaymentStatus.Open;
                    }
                    var ticketType = _context.TicketTypes.Find(source.TicketTypeId);
                    if (ticketType == null)
                    {
                        throw new Exception($"TicketType {source.TicketTypeId} not found.");
                    }
                    if (source.AmountPaid < ticketType.Price)
                    {
                        return PaymentStatus.PaidPartial;
                    }
                    return PaymentStatus.Paid;
                }
                return source.PaymentStatus;
            }
        }
    }
}
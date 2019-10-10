using AutoMapper;
using PaymentStatus = EventManagement.ApplicationCore.Models.PaymentStatus;

namespace EventManagement.WebApp.Mappers
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<ApplicationCore.Models.Ticket, Models.Ticket>()
                .ForMember(e => e.Creator, opt => opt.MapFrom(e => e.Creator.Name))
                .ForMember(e => e.Editor, opt => opt.MapFrom(e => e.Editor.Name))
                .ReverseMap()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.TicketSecret, opt => opt.Ignore())
                .ForMember(e => e.Creator, opt => opt.Ignore())
                .ForMember(e => e.Editor, opt => opt.Ignore())
                .ForMember(e => e.AmountPaid, opt => opt.MapFrom(
                    e => e.PaymentStatus == PaymentStatus.PaidPartial ? e.AmountPaid : null));
        }
    }
}
using AutoMapper;

namespace EventManagement.WebApp.Mappers
{
    public class SupportTicketMapperProfile : Profile
    {
        public SupportTicketMapperProfile()
        {
            CreateMap<ApplicationCore.Models.SupportTicket, Models.SupportTicket>()
                .ForMember(e => e.TicketNumber, opt => opt.MapFrom(e => e.Ticket.TicketNumber))
                .ForMember(e => e.LastName, opt => opt.MapFrom(e => e.Ticket.LastName))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(e => e.Ticket.FirstName));
        }
    }
}
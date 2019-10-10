using AutoMapper;
using System.Linq;

namespace EventManagement.WebApp.Mappers
{
    public class TicketTypeMapperProfile : Profile
    {
        public TicketTypeMapperProfile()
        {
            CreateMap<ApplicationCore.Models.TicketType, Models.TicketType>()
                .ReverseMap();

            CreateMap<ApplicationCore.Models.TicketType, Models.TicketQuotaReportRow>()
                .ForMember(e => e.Count, opt => opt.MapFrom(t => t.Tickets.Count()));
        }
    }
}
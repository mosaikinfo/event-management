using AutoMapper;

namespace EventManagement.WebApp.Mappers
{
    public class TicketTypeMapperProfile : Profile
    {
        public TicketTypeMapperProfile()
        {
            CreateMap<ApplicationCore.Models.TicketType, Models.TicketType>()
                .ReverseMap();
        }
    }
}
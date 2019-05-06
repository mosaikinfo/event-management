using AutoMapper;

namespace EventManagement.WebApp.Mappers
{
    public class TicketTypeMapperProfile : Profile
    {
        public TicketTypeMapperProfile()
        {
            CreateMap<DataAccess.Models.TicketType, Models.TicketType>()
                .ReverseMap();
        }
    }
}
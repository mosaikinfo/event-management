using AutoMapper;

namespace EventManagement.WebApp.Mappers
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            CreateMap<ApplicationCore.Models.Event, Models.Event>()
                .ReverseMap()
                .ForMember(e => e.Id, opt => opt.Ignore());
        }
    }
}
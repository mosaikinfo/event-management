using AutoMapper;
using EventManagement.ApplicationCore.Models.Extensions;

namespace EventManagement.WebApp.Mappers
{
    public class AuditEventMapperProfile : Profile
    {
        public AuditEventMapperProfile()
        {
            CreateMap<ApplicationCore.Models.AuditEvent, Models.AuditEvent>()
                .ForMember(e => e.Level, opt => opt.MapFrom(e => e.Level.GetStringValue()));
        }
    }
}
using AutoMapper;

namespace EventManagement.WebApp.Mappers
{
    public class AuditEventMapperProfile : Profile
    {
        public AuditEventMapperProfile()
        {
            CreateMap<ApplicationCore.Models.AuditEvent, Models.AuditEvent>();
        }
    }
}
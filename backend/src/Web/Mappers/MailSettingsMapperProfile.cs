using AutoMapper;

namespace EventManagement.WebApp.Mappers
{
    public class MailSettingsMapperProfile : Profile
    {
        public MailSettingsMapperProfile()
        {
            CreateMap<ApplicationCore.Models.MailSettings, Models.MailSettings>()
                .ReverseMap();
        }
    }
}
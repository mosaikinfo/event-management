using AutoMapper;
using System.Linq;

namespace EventManagement.WebApp.Mappers
{
    public class MailSettingsMapperProfile : Profile
    {
        public const string PasswordPlaceholder = "*****************";

        public MailSettingsMapperProfile()
        {
            CreateMap<ApplicationCore.Models.MailSettings, Models.MailSettings>()
                .ForMember(x => x.SmtpPassword, opt => opt.MapFrom(
                    x => x.SmtpPassword != null && x.SmtpPassword.Length > 0 ? PasswordPlaceholder : null))
                .ForMember(x => x.DemoEmailRecipients, opt => opt.MapFrom(
                    x => x.DemoEmailRecipients.Select(r => r.EmailAddress).ToList()))
                .ReverseMap()
                .ForMember(x => x.SmtpPassword, opt => opt.Condition(
                    x => !string.Equals(x.SmtpPassword, PasswordPlaceholder)))
                .ForMember(x => x.DemoEmailRecipients, opt => opt.Ignore());
        }
    }
}
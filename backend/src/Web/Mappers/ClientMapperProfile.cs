using AutoMapper;
using IdentityServer4.Models;

namespace EventManagement.WebApp.Mappers
{
    public class ClientMapperProfile : Profile
    {
        private const string PasswordReplacement = "[redacted]";

        public ClientMapperProfile()
        {
            CreateMap<ApplicationCore.Models.Client, Models.Client>()
                .ForMember(e => e.Secret, opt => opt.MapFrom(
                    e => string.IsNullOrEmpty(e.Secret) ? null : PasswordReplacement));

            CreateMap<Models.Client, ApplicationCore.Models.Client>()
                .ForMember(e => e.Secret,
                    opt => opt.Condition(
                        e => e.Secret != null && e.Secret != PasswordReplacement))
                .ForMember(e => e.Secret, opt => opt.MapFrom(e => e.Secret.Sha256()))
                .ForMember(e => e.CreatedAt, opt => opt.Ignore())
                .ForMember(e => e.EditedAt, opt => opt.Ignore());
        }
    }
}
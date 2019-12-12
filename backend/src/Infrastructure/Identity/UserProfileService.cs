using EventManagement.ApplicationCore.Models.Extensions;
using EventManagement.Infrastructure.Data;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.Identity
{
    public class UserProfileService : IProfileService
    {
        private readonly EventsDbContext _dbContext;
        private readonly ILogger _logger;

        public UserProfileService(EventsDbContext dbContext, ILogger<UserProfileService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(_logger);

            if (context.RequestedClaimTypes.Any())
            {
                Guid userId = context.Subject.GetUserId();
                var user = _dbContext.Users.Find(userId);

                if (user != null)
                {
                    context.AddRequestedClaims(new[]
                    {
                        new Claim(JwtClaimTypes.Name, user.Name),
                        new Claim(JwtClaimTypes.Email, user.EmailAddress),
                        new Claim(JwtClaimTypes.Role, user.Role.GetStringValue())
                    });
                }
            }

            context.LogIssuedClaims(_logger);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            Guid userId = context.Subject.GetUserId();
            var user = _dbContext.Users.Find(userId);

            context.IsActive = user?.Enabled == true;

            return Task.CompletedTask;
        }
    }
}
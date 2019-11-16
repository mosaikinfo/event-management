using EventManagement.ApplicationCore.Identity;
using IdentityServer4;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Identity
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IdentityServerTools _tools;

        public JwtTokenService(IdentityServerTools tools)
        {
            _tools = tools;
        }

        public Task<string> IssueJwtAsync(int lifetime, IEnumerable<Claim> claims)
        {
            return _tools.IssueJwtAsync(lifetime, claims);
        }
    }
}
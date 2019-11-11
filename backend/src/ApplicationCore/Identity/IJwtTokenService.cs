using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Identity
{
    /// <summary>
    /// Token service to issue JSON Web Tokens (JWT).
    /// </summary>
    public interface IJwtTokenService
    {
        Task<string> IssueJwtAsync(int lifetime, IEnumerable<Claim> claims);
    }
}
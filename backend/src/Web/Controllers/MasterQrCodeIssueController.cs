using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to issue master qr codes that can be used to 
    /// authenticate a qr code scanner app for validating tickets.
    /// </summary>
    [Route("masterqrcode")]
    [Authorize(AuthenticationSchemes = IdentityServerConstants.DefaultCookieAuthenticationScheme)]
    public class MasterQrCodeIssueController : ControllerBase
    {
        private readonly IdentityServerTools _tools;

        public MasterQrCodeIssueController(IdentityServerTools tools)
        {
            _tools = tools;
        }

        [HttpGet("my")]
        public async Task<IActionResult> IssueMasterQrCodeAsync()
        {
            var token = await GetAccessTokenAsync();

            return Content(token);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            int lifetime = 90 * 24 * 3600; // 90 days.

            string token = await _tools.IssueJwtAsync(lifetime, new[]
            {
                new Claim("sub", "83482392"),
                new Claim("on_behalf_of", "<uid>")
            });

            return token;
        }
    }
}

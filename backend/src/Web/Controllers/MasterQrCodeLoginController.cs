using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [AllowAnonymous]
    public class MasterQrCodeLoginController : ControllerBase
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly ILogger _logger;

        public MasterQrCodeLoginController(ITokenValidator tokenValidator,
                                           ILogger<MasterQrCodeLoginController> logger)
        {
            _tokenValidator = tokenValidator;
            _logger = logger;
        }

        [HttpGet("qrauth/{token}")]
        public async Task<IActionResult> LoginAsync(string token)
        {
            _logger.LogInformation("Validate access token.");

            var tokenResult = await _tokenValidator.ValidateAccessTokenAsync(
                token,
                ApiScopes.EntranceControl.ScanQr);

            if (tokenResult.IsError)
            {
                _logger.LogInformation("Invalid token: {error}", tokenResult.Error);
                return AccessDenied();
            }

            var subClaim = tokenResult.Claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Subject);
            if (subClaim == null)
            {
                _logger.LogInformation("Token contains no sub claim");
                return AccessDenied();
            }

            _logger.LogInformation("Sub claim: {sub}", subClaim.Value);

            return Content(token);
        }

        private IActionResult AccessDenied()
        {
            // TODO: return access denied page.
            return StatusCode((int)HttpStatusCode.Forbidden);
        }
    }
}

using EventManagement.ApplicationCore.Models;
using EventManagement.Infrastructure.Data;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [OpenApiIgnore]
    [AllowAnonymous]
    public class MasterQrCodeLoginController : ControllerBase
    {
        private readonly EventsDbContext _context;
        private readonly ILogger _logger;

        public MasterQrCodeLoginController(EventsDbContext context,
                                           ILogger<MasterQrCodeLoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("qrauth/{token}")]
        public async Task<IActionResult> LoginAsync(string token)
        {
            _logger.LogInformation("Validate token");

            if (!Guid.TryParse(token, out Guid masterQrCodeId))
            {
                _logger.LogWarning("Token is no valid GUID.");
                return AccessDenied();
            }

            MasterQrCode masterQrCode =
                await _context.MasterQrCodes.FindAsync(masterQrCodeId);

            if (masterQrCode == null)
            {
                _logger.LogWarning("Token not found in the database.");
                return AccessDenied();
            }

            if (masterQrCode.RevokedAt != null)
            {
                _logger.LogWarning(
                    "The master qr code has been revoked at {date}",
                    masterQrCode.RevokedAt);
                return AccessDenied();
            }

            await SignInAsync(masterQrCode);

            // TODO: display success page.
            return Content("Login erfolgreich");
        }

        private Task SignInAsync(MasterQrCode masterQrCode)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, masterQrCode.OwnerId.ToString()),
                new Claim(CustomClaimTypes.EventId, masterQrCode.EventId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, Constants.MasterQrCodeAuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            return HttpContext.SignInAsync(
                Constants.MasterQrCodeAuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private IActionResult AccessDenied()
        {
            // TODO: return access denied page.
            return StatusCode((int) HttpStatusCode.Forbidden);
        }
    }
}

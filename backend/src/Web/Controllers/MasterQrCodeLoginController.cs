using EventManagement.ApplicationCore.Models;
using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    [OpenApiIgnore]
    [AllowAnonymous]
    public class MasterQrCodeLoginController : Controller
    {
        public const string LoginRouteName = "QrCodeLogin";
        private readonly EventsDbContext _context;
        private readonly ILogger _logger;

        public MasterQrCodeLoginController(EventsDbContext context,
                                           ILogger<MasterQrCodeLoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("qrauth/{token}", Name = LoginRouteName)]
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
            return LogonSuccess();
        }

        private Task SignInAsync(MasterQrCode masterQrCode)
        {
            string authenticationScheme = EventManagementConstants.MasterQrCode.AuthenticationScheme;
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, masterQrCode.OwnerId.ToString()),
                new Claim(EventManagementClaimTypes.EventId, masterQrCode.EventId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            return HttpContext.SignInAsync(
                authenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private IActionResult LogonSuccess()
        {
            return View("Status", new StatusMessage
            {
                Message = "Anmeldung erfolgreich!",
                IconCssClass = "fas fa-key"
            });
        }

        private IActionResult AccessDenied()
        {
            return View("Status", new StatusMessage
            {
                Message = "Anmeldung fehlgeschlagen!",
                BackgroundCssClass = "bg-danger",
                IconCssClass = "fas fa-key"
            });
        }
    }
}
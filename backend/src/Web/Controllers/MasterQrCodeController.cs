using EventManagement.ApplicationCore.Models;
using EventManagement.Identity;
using EventManagement.Infrastructure.Data;
using EventManagement.Shared.Mvc;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to issue master qr codes that can be used to 
    /// authenticate a qr code scanner app for validating tickets.
    /// </summary>
    [Route("events/{eventId}/masterqrcodes")]
    [Authorize(AuthenticationSchemes = IdentityServerConstants.DefaultCookieAuthenticationScheme)]
    public class MasterQrCodeIssueController : ControllerBase
    {
        private readonly IdentityServerTools _tools;
        private readonly EventsDbContext _context;

        public MasterQrCodeIssueController(IdentityServerTools tools,
                                           EventsDbContext context)
        {
            _tools = tools;
            _context = context;
        }

        [HttpGet]
        [Route("my")]
        [Route("my.png")]
        public async Task<IActionResult> DownloadMyMasterQrCodeAsync(Guid eventId)
        {
            Guid userId = User.GetUserId();
            MasterQrCode masterQrCode = await _context.MasterQrCodes
                .Where(e => e.EventId == eventId &&
                            e.OwnerId == userId &&
                            e.RevokedAt == null)
                .FirstOrDefaultAsync();

            if (masterQrCode == null)
            {
                masterQrCode = new MasterQrCode
                {
                    EventId = eventId,
                    OwnerId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Add(masterQrCode);
                await _context.SaveChangesAsync();
            }

            string token = await IssueJwtAsync(masterQrCode);

            const string placeholder = "__token__";
            string loginUrl = Url.Action(
                "LoginAsync", "MasterQrCodeLogin", new { token = placeholder },
                // makes sure that an absolute url is created.
                Request.Scheme);

            // Url.Action() would convert all to lowercase characters.
            loginUrl = loginUrl.Replace(placeholder, token);

            return new QrCodeResult(loginUrl);
        }

        private Task<string> IssueJwtAsync(MasterQrCode masterQrCode)
        {
            int lifetime = 365 * 24 * 3600; // 365 days.
            return _tools.IssueJwtAsync(lifetime, new[]
            {
                new Claim("sub", masterQrCode.Id.ToString()),
                new Claim("scope", ApiScopes.EntranceControl.ScanQr)
            }); 
        }
    }
}

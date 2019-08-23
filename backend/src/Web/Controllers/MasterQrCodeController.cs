using EventManagement.ApplicationCore.Models;
using EventManagement.Identity;
using EventManagement.Infrastructure.Data;
using EventManagement.Shared.Mvc;
using EventManagement.WebApp.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to issue master qr codes that can be used to
    /// authenticate a qr code scanner app for validating tickets.
    /// </summary>
    [OpenApiIgnore]
    [Route("events/{eventId}/masterqrcodes")]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class MasterQrCodeIssueController : ControllerBase
    {
        private readonly EventsDbContext _context;

        public MasterQrCodeIssueController(EventsDbContext context)
        {
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

            string loginUrl = Url.ActionAbsoluteUrl<MasterQrCodeLoginController>(
                nameof(MasterQrCodeLoginController.LoginAsync),
                new { token = masterQrCode.Id.ToString() });

            return new QrCodeResult(loginUrl);
        }
    }
}
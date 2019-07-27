using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.WebApp.Controllers
{
    [AllowAnonymous]
    public class MasterQrCodeLoginController : ControllerBase
    {
        [HttpGet("qrauth/{token}")]
        public IActionResult Login(string token)
        {
            return Content(token);
        }
    }
}

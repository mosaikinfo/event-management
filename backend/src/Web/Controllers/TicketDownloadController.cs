using EventManagement.ApplicationCore.Exceptions;
using EventManagement.ApplicationCore.TicketGeneration;
using EventManagement.WebApp.Shared.Mvc;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventManagement.WebApp.Controllers
{
    /// <summary>
    /// Controller to download tickets with an internet browser.
    /// </summary>
    [SecurityHeaders]
    [Authorize(EventManagementConstants.AdminApi.PolicyName)]
    public class TicketDownloadController : EventManagementController
    {
        private readonly IPdfTicketService _pdfTicketService;

        public TicketDownloadController(IPdfTicketService pdfTicketService)
        {
            _pdfTicketService = pdfTicketService;
        }

        /// <summary>
        /// Download a ticket as pdf.
        /// </summary>
        /// <param name="id">Id of the ticket.</param>
        /// <returns>pdf file</returns>
        [HttpGet("tickets/{id}/pdf")]
        public async Task<IActionResult> DownloadAsPdf(Guid id)
        {
            string validationUrl = GetTicketValidationUriFormatString();
            Stream stream;
            try
            {
                stream = await _pdfTicketService.GeneratePdfAsync(id, validationUrl);
            }
            catch (TicketNotFoundException)
            {
                return NotFound();
            }
            string fileDownloadName = "ticket.pdf";
            Response.Headers.Add("Content-Disposition", $"inline; filename={fileDownloadName}");
            return File(stream, "application/pdf");
        }
    }
}
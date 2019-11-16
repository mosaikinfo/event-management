using ServiceStack;
using ServiceStack.Script;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.TicketDelivery
{
    public static class EmailTemplateService
    {
        /// <summary>
        /// Render the gienv #script e-mail template.
        /// </summary>
        /// <param name="mail">text email message using the #script language within the message body and subject.</param>
        /// <param name="ticket">Ticket data to populate the variables that can be used within the template.</param>
        /// <returns>rendered template</returns>
        public static async Task<EmailMessage> RenderTicketMailAsync(
            EmailMessage mail, Models.Ticket ticket, string homepageUrl)
        {
            var context = new ScriptContext
            {
                PageFormats = { new PlainTextPageFormat() },
                Args =
                {
                    ["FirstName"] = ticket.FirstName,
                    ["LastName"] = ticket.LastName,
                    ["TicketTypeName"] = ticket.TicketType.Name,
                    ["TicketPrice"] = ticket.TicketType.Price,
                    ["EventName"] = ticket.Event.Name,
                    ["EventLocation"] = ticket.Event.Location,
                    ["EventHomepageUrl"] = homepageUrl,
                    ["EventHost"] = ticket.Event.Host
                }
            };
            context.Init();
            mail.Subject = await context.RenderScriptAsync(mail.Subject);
            mail.Body = await context.RenderScriptAsync(mail.Body);
            return mail;
        }

        public class PlainTextPageFormat : PageFormat
        {
            public PlainTextPageFormat()
            {
                Extension = "txt";
                ContentType = MimeTypes.PlainText;
            }
        }
    }
}
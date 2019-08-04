using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace EventManagement.WebApp.Shared.Mvc
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates an absolute url to the controller action.
        /// </summary>
        public static string ActionAbsoluteUrl<TController>(
            this IUrlHelper helper, string action, object values)
            where TController : ControllerBase
        {
            // this makes sure that an absolute url is created.
            string protocol = helper.ActionContext.HttpContext.Request.Scheme;

            string controllerName = Regex.Replace(
                typeof(TController).Name, "controller$", "", RegexOptions.IgnoreCase);

            return helper.Action(action, controllerName, values, protocol);
        }
    }
}

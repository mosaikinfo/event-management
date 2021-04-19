using Microsoft.AspNetCore.Mvc;

namespace EventManagement.WebApp.Shared.Mvc
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates an absolute url for the given named route.
        /// </summary>
        /// <remarks>
        /// You have to use the Route-attribute in the controller class specifying a name for the route.
        /// Example:
        /// <c>[Route("foo/bar", Name = "routeName")]</c>
        /// </remarks>
        public static string RouteAbsoluteUrl(this IUrlHelper helper, string routeName, object values)
        {
            // this makes sure that an absolute url is created.
            string protocol = helper.ActionContext.HttpContext.Request.Scheme;

            return helper.RouteUrl(new Microsoft.AspNetCore.Mvc.Routing.UrlRouteContext
            {
                Protocol = protocol,
                RouteName = routeName,
                Values = values
            });
        }
    }
}
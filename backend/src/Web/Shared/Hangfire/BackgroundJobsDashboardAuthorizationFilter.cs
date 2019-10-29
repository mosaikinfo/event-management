using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static EventManagement.EventManagementConstants;

namespace EventManagement.WebApp.Shared.Hangfire
{
    public class BackgroundJobsDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public BackgroundJobsDashboardAuthorizationFilter(IHttpContextAccessor httpContextAccessor,
                                                          IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var authorizeResult = _authorizationService
                .AuthorizeAsync(
                    _httpContextAccessor.HttpContext.User,
                    AdminApi.PolicyName)
                .GetAwaiter().GetResult();

            return authorizeResult.Succeeded;


        }
    }
}

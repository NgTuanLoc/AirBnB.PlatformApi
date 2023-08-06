using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlatformApi.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private const string HeaderKey = "Authorization";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // if (ShouldSkipAuthorization(context))
            // {
            //     return; // Skip authorization
            // }

            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderKey, out var requestValue))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = $"Bearer {configuration["ApiKey"]}";

            if (apiKey == null || !apiKey.Equals(requestValue))
            {
                context.Result = new UnauthorizedResult();
            }
        }
        private static bool ShouldSkipAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
            {
                // Check if the action is marked with [AllowAnonymous]
                return true;
            }

            return false;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlatformApi.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private const string HeaderKey = "Authorization";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
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
    }
}
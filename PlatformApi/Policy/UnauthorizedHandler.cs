using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace PlatformApi.Policy
{
   public class UnauthorizedHandler : IAuthorizationMiddlewareResultHandler
   {
      public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
      {
         if (context.User.Identity?.IsAuthenticated != true)
         {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("You need to login");
            return;
         }
         await next(context);
      }
   }
}
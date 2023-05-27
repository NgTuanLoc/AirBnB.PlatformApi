using System.Net;
using Core.Exceptions;
using Newtonsoft.Json;

namespace PlatformApi.Middlewares
{
   public class ErrorHandlingMiddleware
   {
      private readonly RequestDelegate _next;

      public ErrorHandlingMiddleware(RequestDelegate next)
      {
         _next = next;
      }

      public async Task InvokeAsync(HttpContext context)
      {
         try
         {
            await _next(context);
         }
         catch (ValidationException ex)
         {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errorResponse = "ValidationException";
            if (ex.Failures.Count > 0)
            {
               errorResponse = JsonConvert.SerializeObject(new { errors = ex.Failures });
            }
            else
            {
               errorResponse = JsonConvert.SerializeObject(new { errors = ex.Message });
            }
            // Return a JSON response with the error message
            await context.Response.WriteAsync(errorResponse);
         }
         catch (NotFoundException ex)
         {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = ex.Message }));
         }
         catch (Exception ex)
         {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = ex.Message }));
         }
      }
   }
}
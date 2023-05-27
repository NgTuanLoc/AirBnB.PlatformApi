using System.Net;
using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlatformApi.Filters
{
   public class CustomExceptionFilter : IExceptionFilter
   {
      public void OnException(ExceptionContext context)
      {
         // Log the exception or perform other custom logic
         if (context.Result != null)
         {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new JsonResult(context.Result);
            return;
         }
      }
   }
}
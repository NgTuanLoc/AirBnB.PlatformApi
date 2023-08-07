using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlatformApi.Filters
{
   public class ModelStateFilter : IActionFilter
   {
      public void OnActionExecuting(ActionExecutingContext context)
      {
         if (!context.ModelState.IsValid)
         {
            var errors = new Dictionary<string, string[]>();

            foreach (var state in context.ModelState)
            {
               var error = state.Key;
               var ErrorMessageList = state.Value.Errors.Select(e => e.ErrorMessage).ToArray();
               if (ErrorMessageList.Length != 0)
               {
                  errors.Add(error, ErrorMessageList);
               }
            }
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(errors);
            return;
         }
      }
      public void OnActionExecuted(ActionExecutedContext context)
      {
         // Do nothing
      }
   }
}
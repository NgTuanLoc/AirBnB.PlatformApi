using Microsoft.AspNetCore.Http;

namespace Core.Models.Location
{
   public class UpdateLocationRequest
   {
      public string? Name { get; set; }
      public string? Province { get; set; }
      public string? Country { get; set; }
      public IFormFile? File { get; set; }
   }
}
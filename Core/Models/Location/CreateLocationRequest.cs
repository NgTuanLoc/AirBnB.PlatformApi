using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Location
{
   public class CreateLocationRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Name { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Province { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Country { get; set; } = default!;
      public IFormFile? File { get; set; }
   }
}
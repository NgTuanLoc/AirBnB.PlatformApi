using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Image
{
   public class UpdateImageRequest
   {
      public string? Title { get; set; }
      public string? Description { get; set; }
      public IFormFile? File { get; set; }
      public Guid? RoomId { get; set; }
   }
}
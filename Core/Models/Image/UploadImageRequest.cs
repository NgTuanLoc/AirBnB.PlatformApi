using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Image
{
   public class UploadImageRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Title { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Description { get; set; } = default!;
      [Display(Name = "File")]
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public IFormFile File { get; set; } = default!;
   }
}
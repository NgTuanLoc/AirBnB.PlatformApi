using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Models.Image
{
   public class UploadImageRequest
   {
      [Display(Name = "FileName")]
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string FileName { get; set; } = default!;
      [Display(Name = "Description")]
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Description { get; set; } = default!;
      [Display(Name = "File")]
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public IFormFile File { get; set; } = default!;
   }
}
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Review
{
   public class CreateReviewRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Title { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Comment { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [Range(1, 5, ErrorMessage = "{0} can not be empty or null")]
      public int Rating { get; set; }
   }
}
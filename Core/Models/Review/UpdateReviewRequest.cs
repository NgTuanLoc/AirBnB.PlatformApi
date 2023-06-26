using System.ComponentModel.DataAnnotations;

namespace Core.Models.Review
{
   public class UpdateReviewRequest
   {
      public string? Title { get; set; }
      public string? Comment { get; set; }
      [Range(1, 5, ErrorMessage = "{0} Invalid rating value")]
      public int Rating { get; set; }
   }
}
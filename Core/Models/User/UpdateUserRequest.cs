using System.ComponentModel.DataAnnotations;

namespace Core.Models.User
{
   public class UpdateUserRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
      public string Email { get; set; } = default!;
      public string? PersonName { get; set; }
      public string? Address { get; set; }
      public string? ProfileImage { get; set; }
      public string? Description { get; set; }
      public bool? IsMarried { get; set; }
   }
}
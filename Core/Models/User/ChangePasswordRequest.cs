using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Core.Models.User
{
   public class ChangePasswordRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [StringLength(40, MinimumLength = 5, ErrorMessage = "{0} should be between {2} and {1} characters long")]
      [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "{0} should be more complex")]
      public string CurrentPassword { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [StringLength(40, MinimumLength = 5, ErrorMessage = "{0} should be between {2} and {1} characters long")]
      [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "{0} should be more complex")]
      public string NewPassword { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [Compare("NewPassword", ErrorMessage = "{0} and confirm password do not match")]
      public string ConfirmPassword { get; set; } = default!;
   }
}
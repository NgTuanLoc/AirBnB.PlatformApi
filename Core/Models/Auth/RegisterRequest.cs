using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Core.Models.Auth
{
   public class RegisterRequest
   {
      // PersonName, Email, Phone, Password, ConfirmPassword
      [Display(Name = "Name")]
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters long")]
      public string PersonName { get; set; } = default!;

      [Required(ErrorMessage = "{0} can not be empty or null")]
      [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
      public string Email { get; set; } = default!;

      [RegularExpression(RegexConstants.PhoneRegex, ErrorMessage = "{0} should contain numbers only")]
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Phone { get; set; } = default!;

      [Required(ErrorMessage = "{0} can not be empty or null")]
      [StringLength(40, MinimumLength = 5, ErrorMessage = "{0} should be between {2} and {1} characters long")]
      [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "{0} should be more complex")]
      public string Password { get; set; } = default!;


      [Required(ErrorMessage = "{0} can not be empty or null")]
      [Compare("Password", ErrorMessage = "{0} and confirm password do not match")]
      public string ConfirmPassword { get; set; } = default!;
   }
}
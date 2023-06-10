using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Constants;

namespace Core.Models.User
{
   public class ResetPasswordRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
      public string Email { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [StringLength(40, MinimumLength = 5, ErrorMessage = "{0} should be between {2} and {1} characters long")]
      [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "{0} should be more complex")]
      public string NewPassword { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [Compare("NewPassword", ErrorMessage = "{0} and confirm password do not match")]
      public string ConfirmPassword { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string ResetPasswordToken { get; set; } = default!;
   }
}
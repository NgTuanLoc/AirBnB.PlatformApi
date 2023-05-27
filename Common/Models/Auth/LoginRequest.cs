using System.ComponentModel.DataAnnotations;

namespace Common.Models.Auth
{
   public class LoginRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
      public string Email { get; set; } = default!;


      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Password { get; set; } = default!;
   }
}
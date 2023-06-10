using System.ComponentModel.DataAnnotations;
using Core.Domain.IdentityEntities;
using Core.Models.User;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class UserController : Controller
   {
      private readonly ILogger<UserController> _logger;
      private readonly IUserService _userService;
      private readonly UserManager<ApplicationUser> _userManager;
      public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, IUserService userService)
      {
         _logger = logger;
         _userManager = userManager;
         _userService = userService;
      }
      [HttpGet("get-user")]
      public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
      {
         var user = await _userManager.GetUserAsync(User);

         if (user == null)
         {
            throw new ValidationException("User not found !");
         }

         return Ok(user);
      }
      [HttpPost("change-password")]
      public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
      {
         var user = await _userManager.GetUserAsync(User);
         var result = await _userService.ChangePasswordService(request, user, cancellationToken);
         return Ok(result);
      }
      [HttpPost("get-reset-password-token")]
      public async Task<IActionResult> GetResetPasswordToken()
      {
         var user = await _userManager.GetUserAsync(User);
         var token = await _userService.GetResetPasswordTokenService(user);
         return Ok(token);
      }
      [HttpPost("reset-password")]
      public async Task<IActionResult> Resetpassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
      {
         var result = await _userService.ResetPasswordService(request, cancellationToken);
         return Ok(result);
      }
      [HttpPatch("update-user")]
      public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
      {
         var result = await _userService.UpdateUserService(request, cancellationToken);
         return Ok(result);
      }
   }
}
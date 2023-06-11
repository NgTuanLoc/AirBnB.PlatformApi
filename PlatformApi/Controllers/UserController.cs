using System.ComponentModel.DataAnnotations;
using Core.Domain.IdentityEntities;
using Core.Models.User;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class UserController : Controller
   {
      private readonly ILogger<UserController> _logger;
      private readonly IUserService _userService;
      public UserController(ILogger<UserController> logger, IUserService userService)
      {
         _logger = logger;
         _userService = userService;
      }
      [HttpGet("get-user")]
      public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
      {
         var user = await _userService.GetUser();
         return Ok(user);
      }
      [HttpPost("change-password")]
      public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
      {
         var result = await _userService.ChangePasswordService(request, cancellationToken);
         return Ok(result);
      }
      [HttpPost("get-reset-password-token")]
      public async Task<IActionResult> GetResetPasswordToken()
      {
         var token = await _userService.GetResetPasswordTokenService();
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
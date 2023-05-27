using Common.Models.Auth;
using Core.Domain.IdentityEntities;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   public class AuthController : Controller
   {
      private readonly ILogger<AuthController> _logger;
      private readonly IAuthService _authService;
      public AuthController(ILogger<AuthController> logger, IAuthService authService)
      {
         _logger = logger;
         _authService = authService;
      }
      [AllowAnonymous]
      [HttpPost("register")]
      public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
      {
         var result = await _authService.RegisterService(request, cancellationToken);
         return Ok(result);
      }

      [AllowAnonymous]
      [HttpPost("login")]
      public async Task<ApplicationUser> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
      {
         var result = await _authService.LoginService(request, cancellationToken);
         return result;
      }

      [AllowAnonymous]
      [HttpDelete("logout")]
      public async Task<string> Logout(CancellationToken cancellationToken)
      {
         var result = await _authService.LogoutService(cancellationToken);
         return result;
      }
   }
}
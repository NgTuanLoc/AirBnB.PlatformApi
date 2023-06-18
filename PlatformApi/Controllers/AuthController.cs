using Core.Models.Auth;
using Core.Domain.IdentityEntities;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   public class AuthController : Controller
   {
      private readonly ILogger<AuthController> _logger;
      private readonly IAuthService _authService;
      private readonly IDatabaseService _databaseService;
      public AuthController(ILogger<AuthController> logger, IAuthService authService, IDatabaseService databaseService)
      {
         _logger = logger;
         _authService = authService;
         _databaseService = databaseService;
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
      public async Task<IActionResult> Logout(CancellationToken cancellationToken)
      {
         var result = await _authService.LogoutService(cancellationToken);
         return Ok(result);
      }

      [Authorize]
      [HttpPost("restore")]
      public async Task<IActionResult> Restore(CancellationToken cancellationToken)
      {
         var result = await _databaseService.RestoreService(cancellationToken);
         return Ok(result);
      }

      [Authorize]
      [HttpPost("seed")]
      public async Task<IActionResult> Seed(CancellationToken cancellationToken)
      {
         return Ok("Seed");
      }
   }
}
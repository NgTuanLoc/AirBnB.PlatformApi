using Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class RoomController : Controller
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly ILogger<RoomController> _logger;
      public RoomController(ILogger<RoomController> logger, UserManager<ApplicationUser> userManager)
      {
         _logger = logger;
         _userManager = userManager;
      }
      [HttpGet("getRoomList")]
      public async Task<IActionResult> GetRoomList()
      {
         var user = await _userManager.GetUserAsync(User);
         return Ok(user);
      }
   }
}
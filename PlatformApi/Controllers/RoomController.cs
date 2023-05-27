using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Authorize]
   public class RoomController : Controller
   {
      private readonly ILogger<RoomController> _logger;
      public RoomController(ILogger<RoomController> logger)
      {
         _logger = logger;
      }
      [HttpGet("getRoomList")]
      public IActionResult GetRoomList()
      {
         return Ok("GetRoomList");
      }
   }
}
using Core.Domain.IdentityEntities;
using Core.Models.Room;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class RoomController : Controller
   {
      private readonly ILogger<RoomController> _logger;
      private readonly IRoomService _roomService;
      public RoomController(ILogger<RoomController> logger, IRoomService roomService)
      {
         _logger = logger;
         _roomService = roomService;
      }
      [HttpGet]
      public async Task<IActionResult> GetAllRoomList(CancellationToken cancellationToken)
      {
         return Ok("GetAllRoomList");
      }
      [HttpGet("{id}")]
      public async Task<IActionResult> GetRoomById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         return Ok("GetRoomById");
      }
      [HttpPost]
      public async Task<IActionResult> CreateRoom([FromForm] CreateRoomRequest request, CancellationToken cancellationToken)
      {
         var result = await _roomService.CreateRoomService(request, cancellationToken);
         return Ok(result);
      }
      [HttpPatch("{id}")]
      public async Task<IActionResult> UpdateRoomById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         return Ok("UpdateRoomById");
      }
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteRoomById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         return Ok("DeleteRoomById");
      }
   }
}
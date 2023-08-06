using Core.Models.Reservation;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class ReservationController : Controller
   {
      private readonly ILogger<ReservationController> _logger;
      private readonly IReservationService _reservationService;
      public ReservationController(ILogger<ReservationController> logger, IReservationService reservationService)
      {
         _logger = logger;
         _reservationService = reservationService;
      }

      [HttpGet]
      public async Task<IActionResult> GetAllReservationListByRoomId([FromQuery] Guid roomId, CancellationToken cancellationToken)
      {
         var result = await _reservationService.GetAllReservationByRoomIdService(roomId, cancellationToken);
         return Ok(result);
      }
      [HttpPost]
      public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequest request, CancellationToken cancellationToken)
      {
         var result = await _reservationService.CreateReservationService(request, cancellationToken);
         return Ok(result);
      }
      [HttpPatch("{id}")]
      public async Task<IActionResult> UpdateReservationById([FromRoute] Guid id, [FromBody] UpdateReservationRequest request, CancellationToken cancellationToken)
      {
         var result = await _reservationService.UpdateReservationByIdService(id, request, cancellationToken);
         return Ok(result);
      }
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteReservationById(Guid id, CancellationToken cancellationToken)
      {
         var result = await _reservationService.DeleteReservationByIdService(id, cancellationToken);
         return Ok(result);
      }
   }
}
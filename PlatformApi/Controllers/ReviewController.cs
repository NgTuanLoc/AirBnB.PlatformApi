using Core.Models.Review;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class ReviewController : Controller
   {
      private readonly ILogger<ReviewController> _logger;
      private readonly IReviewService _reviewService;
      public ReviewController(ILogger<ReviewController> logger, IReviewService reviewService)
      {
         _logger = logger;
         _reviewService = reviewService;
      }
      [HttpGet]
      public async Task<IActionResult> GetAllReviewsByRoomId([FromQuery] Guid roomId, CancellationToken cancellationToken)
      {
         var result = await _reviewService.GetAllReviewsByRoomIdService(roomId, cancellationToken);
         return Ok(result);
      }
      [HttpPost]
      public async Task<IActionResult> CreateReviewByRoomId([FromQuery] Guid roomId, [FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
      {
         var result = await _reviewService.CreateReviewByRoomIdService(request, roomId, cancellationToken);
         return Ok(result);
      }
      [HttpPatch("{id}")]
      public async Task<IActionResult> UpdateReviewById([FromRoute] Guid id, [FromBody] UpdateReviewRequest request, CancellationToken cancellationToken)
      {
         var result = await _reviewService.UpdateReviewByIdService(request, id, cancellationToken);
         return Ok(result);
      }
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteReviewById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         var result = await _reviewService.DeleteReviewByIdService(id, cancellationToken);
         return Ok(result);
      }
   }
}
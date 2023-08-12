using Core.Models.Image;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [ApiVersion("1.0")]
   [Authorize]
   public class ImageController : BaseController
   {
      private readonly ILogger<ImageController> _logger;
      private readonly IImageService _imageService;
      public ImageController(ILogger<ImageController> logger, IImageService imageService)
      {
         _logger = logger;
         _imageService = imageService;
      }
      [HttpGet("{id}")]
      public async Task<IActionResult> GetImageById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         var result = await _imageService.GetImageByIdService(id, cancellationToken);
         return Ok(result);
      }
      [HttpPost("upload-image")]
      public async Task<IActionResult> CreateImage([FromForm] UploadImageRequest request, CancellationToken cancellationToken)
      {
         var result = await _imageService.CreateImageService(request, cancellationToken);
         return Ok(result);
      }
      [HttpPatch("{id}")]
      public async Task<IActionResult> UpdateImageById([FromRoute] Guid id, [FromForm] UpdateImageRequest request, CancellationToken cancellationToken)
      {
         var result = await _imageService.UpdateImageByIdService(id, request, cancellationToken);
         return Ok(result);
      }
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteImageById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         var result = await _imageService.DeleteImageByIdService(id, cancellationToken);
         return Ok(result);
      }
   }
}
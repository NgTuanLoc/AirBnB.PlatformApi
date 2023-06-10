using Core.Models.Image;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class ImageController : Controller
   {
      private readonly ILogger<ImageController> _logger;
      private readonly IImageService _imageService;
      public ImageController(ILogger<ImageController> logger, IImageService imageService)
      {
         _logger = logger;
         _imageService = imageService;
      }
      [HttpPost("upload-image")]
      public async Task<IActionResult> CreateImage([FromForm] UploadImageRequest request, CancellationToken cancellationToken)
      {
         var result = await _imageService.CreateImageAsync(request, cancellationToken);
         return Ok(result);
      }
   }
}
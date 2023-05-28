using Common.Models.Image;
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
      public async Task<IActionResult> CreateImage([FromForm] UploadImageRequest request)
      {
         await _imageService.CreateImageAsync(request);
         return Ok("createImage successfully");
      }
   }
}
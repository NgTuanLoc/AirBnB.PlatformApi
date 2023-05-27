using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Authorize]
   public class ImageController : Controller
   {
      private readonly ILogger<ImageController> _logger;
      public ImageController(ILogger<ImageController> logger)
      {
         _logger = logger;
      }
      [HttpPost("createImage")]
      public async Task<IActionResult> CreateImage()
      {
         return Ok("createImage successfully");
      }
   }
}
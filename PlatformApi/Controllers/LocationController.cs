using Core.Models.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [Route("api/v1/[controller]")]
   [Authorize]
   public class LocationController : Controller
   {
      private readonly ILogger<LocationController> _logger;
      public LocationController(ILogger<LocationController> logger)
      {
         _logger = logger;
      }
      [HttpGet]
      public async Task<IActionResult> GetAllLocationList(CancellationToken cancellationToken)
      {
         return Ok("GetAllLocationList");
      }
      [HttpGet("{id}")]
      public async Task<IActionResult> GetLocationById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         return Ok("GetLocationById");
      }
      [HttpPost]
      public async Task<IActionResult> CreateLocation([FromForm] CreateLocationRequest request, CancellationToken cancellationToken)
      {
         return Ok("CreateLocation");
      }
      [HttpPatch("{id}")]
      public async Task<IActionResult> UpdateLocationById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         return Ok("UpdateLocationById");
      }
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteLocationById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         return Ok("DeleteLocationById");
      }
   }
}
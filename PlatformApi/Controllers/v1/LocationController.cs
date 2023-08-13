using Core.Models.Location;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
   [ApiVersion("1.0")]
   [Authorize]
   public class LocationController : BaseController
   {
      private readonly ILogger<LocationController> _logger;
      private readonly ILocationService _locationService;
      public LocationController(ILogger<LocationController> logger, ILocationService locationService)
      {
         _logger = logger;
         _locationService = locationService;
      }
      [AllowAnonymous]
      [HttpGet]
      public async Task<IActionResult> GetAllLocationList(CancellationToken cancellationToken)
      {
         var result = await _locationService.GetAllLocationService(cancellationToken);
         return Ok(result);
      }
      [HttpGet("{id}")]
      public async Task<IActionResult> GetLocationById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         var result = await _locationService.GetLocationByIdService(id, cancellationToken);
         return Ok(result);
      }
      [HttpPost]
      public async Task<IActionResult> CreateLocation([FromForm] CreateLocationRequest request, CancellationToken cancellationToken)
      {
         var result = await _locationService.CreateLocationService(request, cancellationToken);
         return Ok(result);
      }
      [HttpPatch("{id}")]
      public async Task<IActionResult> UpdateLocationById([FromRoute] Guid id, [FromForm] UpdateLocationRequest request, CancellationToken cancellationToken)
      {
         var result = await _locationService.UpdateLocationByIdService(id, request, cancellationToken);
         return Ok(result);
      }
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteLocationById([FromRoute] Guid id, CancellationToken cancellationToken)
      {
         var result = await _locationService.DeleteLocationByIdService(id, cancellationToken);
         return Ok(result);
      }
   }
}
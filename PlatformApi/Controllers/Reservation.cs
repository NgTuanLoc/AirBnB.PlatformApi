using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ILogger<ReservationController> _logger;
        
        public ReservationController(ILogger<ReservationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservationList(CancellationToken cancellationToken)
        {
            return Ok("GetAllReservationList");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(Guid id, CancellationToken cancellationToken)
        {
            return Ok("GetReservationById");
        } 
        [HttpPost]
        public async Task<IActionResult> CreateReservation(CancellationToken cancellationToken)
        {
            return Ok("CreateReservation");
        } 
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateReservationById(Guid id, CancellationToken cancellationToken)
        {
            return Ok("UpdateReservationById");
        } 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservationById(Guid id, CancellationToken cancellationToken)
        {
            return Ok("DeleteReservationById");
        } 
    }
}
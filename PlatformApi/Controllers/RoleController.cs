using Core.Models.Role;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        public RoleController(ILogger<RoleController> logger, IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }
        [HttpGet]
        public IActionResult GetAllRoleList()
        {
            var result = _roleService.GetAllRoleListService();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetRoleByIdService(id, cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.CreateRoleService(request, cancellationToken);
            return Ok(result);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRoleById([FromRoute] Guid id, [FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.UpdateRoleByIdService(id, request, cancellationToken);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _roleService.DeleteRoleByIdService(id, cancellationToken);
            return Ok(result);
        }
    }
}
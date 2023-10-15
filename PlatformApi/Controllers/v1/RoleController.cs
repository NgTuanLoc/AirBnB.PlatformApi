using Core.Constants;
using Core.Models.Role;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoleOptions.ADMIN)]
    public class RoleController : BaseController
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
        [HttpPatch("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser(CancellationToken cancellationToken)
        {
            return Ok("AssignRoleToUser");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _roleService.DeleteRoleByIdService(id, cancellationToken);
            return Ok(result);
        }
    }
}
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
        public async Task<IActionResult> GetAllRoleList(CancellationToken cancellationToken)
        {
            return Ok("GetAllRoleList");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Ok("GetRoleById");
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CancellationToken cancellationToken)
        {
            return Ok("CreateRole");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRoleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Ok("UpdateRoleById");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Ok("DeleteRoleById");
        }
    }
}
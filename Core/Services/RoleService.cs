using System.ComponentModel.DataAnnotations;
using Core.Domain.IdentityEntities;
using Core.Models.Role;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public interface IRoleService
    {
        List<CreateRoleResponse> GetAllRoleListService();
        Task<CreateRoleResponse> GetRoleByIdService(Guid id, CancellationToken cancellationToken);
        Task<CreateRoleResponse> CreateRoleService(CreateRoleRequest request, CancellationToken cancellationToken);
        Task<List<CreateRoleResponse>> CreateRoleListService(List<CreateRoleRequest> request, CancellationToken cancellationToken);
        Task<CreateRoleResponse> UpdateRoleByIdService(Guid id, CreateRoleRequest request, CancellationToken cancellationToken);
        Task<CreateRoleResponse> DeleteRoleByIdService(Guid id, CancellationToken cancellationToken);

    }
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<CreateRoleResponse> CreateRoleService(CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var existedRole = await _roleManager.FindByNameAsync(request.RoleName);

            if (existedRole != null) throw new ValidationException("Role already existed!");

            var newRole = new ApplicationRole()
            {
                Name = request.RoleName
            };
            await _roleManager.CreateAsync(newRole);

            return ConvertApplicationRoleIntoCreateRoleResponse(newRole);
        }

        public async Task<CreateRoleResponse> DeleteRoleByIdService(Guid id, CancellationToken cancellationToken)
        {
            var existedRole = await _roleManager.FindByIdAsync(id.ToString());

            if (existedRole == null) throw new ValidationException("Role does not existed!");

            await _roleManager.DeleteAsync(existedRole);

            return ConvertApplicationRoleIntoCreateRoleResponse(existedRole);
        }

        public List<CreateRoleResponse> GetAllRoleListService()
        {
            var roleList = _roleManager.Roles.ToList();
            var roleListResponse = new List<CreateRoleResponse>();

            foreach (var role in roleList)
            {
                roleListResponse.Add(ConvertApplicationRoleIntoCreateRoleResponse(role));
            }

            return roleListResponse;
        }

        public async Task<CreateRoleResponse> GetRoleByIdService(Guid id, CancellationToken cancellationToken)
        {
            var existedRole = await _roleManager.FindByIdAsync(id.ToString());

            if (existedRole == null) throw new ValidationException("Role does not existed!");

            return ConvertApplicationRoleIntoCreateRoleResponse(existedRole);
        }

        public async Task<CreateRoleResponse> UpdateRoleByIdService(Guid id, CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var existedRole = await _roleManager.FindByIdAsync(id.ToString());

            if (existedRole == null) throw new ValidationException("Role does not existed!");

            existedRole.Name = request.RoleName;
            await _roleManager.UpdateAsync(existedRole);

            return ConvertApplicationRoleIntoCreateRoleResponse(existedRole);
        }

        public async Task<List<CreateRoleResponse>> CreateRoleListService(List<CreateRoleRequest> request, CancellationToken cancellationToken)
        {
            List<CreateRoleResponse> response = new List<CreateRoleResponse>();

            foreach (var role in request)
            {
                var result = await CreateRoleService(role, cancellationToken);
                response.Add(result);
            }
            return response;
        }

        private static CreateRoleResponse ConvertApplicationRoleIntoCreateRoleResponse(ApplicationRole role)
        {
            return new CreateRoleResponse()
            {
                Id = role.Id,
                RoleName = role.Name ?? "Unknown",
                NormalizedRoleName = role.NormalizedName ?? "Unknown"
            };
        }
    }
}
using System.ComponentModel.DataAnnotations;
using AutoMapper;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
            var response = _mapper.Map<ApplicationRole, CreateRoleResponse>(newRole);

            return response;
        }

        public async Task<CreateRoleResponse> DeleteRoleByIdService(Guid id, CancellationToken cancellationToken)
        {
            var existedRole = await _roleManager.FindByIdAsync(id.ToString()) ?? throw new ValidationException("Role does not existed!");
            await _roleManager.DeleteAsync(existedRole);
            var response = _mapper.Map<ApplicationRole, CreateRoleResponse>(existedRole);

            return response;
        }

        public List<CreateRoleResponse> GetAllRoleListService()
        {
            var roleList = _roleManager.Roles.ToList();
            var roleListResponse = new List<CreateRoleResponse>();

            foreach (var role in roleList)
            {
                roleListResponse.Add(_mapper.Map<ApplicationRole, CreateRoleResponse>(role));
            }

            return roleListResponse;
        }

        public async Task<CreateRoleResponse> GetRoleByIdService(Guid id, CancellationToken cancellationToken)
        {
            var existedRole = await _roleManager.FindByIdAsync(id.ToString()) ?? throw new ValidationException("Role does not existed!");
            var response = _mapper.Map<ApplicationRole, CreateRoleResponse>(existedRole);

            return response;
        }

        public async Task<CreateRoleResponse> UpdateRoleByIdService(Guid id, CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var existedRole = await _roleManager.FindByIdAsync(id.ToString()) ?? throw new ValidationException("Role does not existed!");
            existedRole.Name = request.RoleName;
            await _roleManager.UpdateAsync(existedRole);
            var response = _mapper.Map<ApplicationRole, CreateRoleResponse>(existedRole);

            return response;
        }

        public async Task<List<CreateRoleResponse>> CreateRoleListService(List<CreateRoleRequest> request, CancellationToken cancellationToken)
        {
            List<CreateRoleResponse> response = new();

            foreach (var role in request)
            {
                var result = await CreateRoleService(role, cancellationToken);
                response.Add(result);
            }
            return response;
        }
    }
}
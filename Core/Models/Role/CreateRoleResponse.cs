using System.ComponentModel.DataAnnotations;

namespace Core.Models.Role
{
    public class CreateRoleResponse
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = default!;
        public string NormalizedRoleName { get; set; } = default!;
    }
}
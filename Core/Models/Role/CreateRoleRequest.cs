using System.ComponentModel.DataAnnotations;

namespace Core.Models.Role
{
    public class CreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; } = default!;
    }
}
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Role
{
    public class CreateRoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string NormalizedName { get; set; } = default!;
    }
}
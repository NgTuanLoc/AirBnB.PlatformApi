namespace Core.Models.User
{
    public class CreateUserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = "Unknown";
        public string PersonName { get; set; } = "Unknown";
        public string Address { get; set; } = "Unknown Address";
        public string ProfileImage { get; set; } = "Unknown ProfileImage";
        public string Description { get; set; } = "Unknown Description";
        public bool? IsMarried { get; set; }
        public string PhoneNumber { get; set; } = "Unknown PhoneNumber";
        public ICollection<string> RoleList { get; set; } = new List<string>();
    }
}
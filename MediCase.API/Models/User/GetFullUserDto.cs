using MediCase.API.Models.Group;

namespace MediCase.API.Models.User
{
    public class GetFullUserDto
    {
        public uint Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = new List<string>();
        public List<GetGroupDto>? Groups { get; set; }
    }
}

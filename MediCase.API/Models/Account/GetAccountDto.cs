using MediCase.API.Models.Group;

namespace MediCase.API.Models.Account
{
    public class GetAccountDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string>? Roles { get; set; }
    }
}

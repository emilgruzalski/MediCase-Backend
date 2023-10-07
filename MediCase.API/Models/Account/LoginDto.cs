namespace MediCase.API.Models.Account
{
    public class LoginDto
    {
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}

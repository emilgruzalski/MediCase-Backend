using MediCase.API.Models.Account;
using MediCase.API.Models.User;

namespace MediCase.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> GenerateJwtAsync(LoginDto dto);
        Task RegisterUserAsync(UserDto dto);
        Task UpdateNameAsync(uint id, UpdateNameDto dto);
        Task UpdateEmailAsync(uint id, UpdateEmailDto dto);
        Task UpdatePasswordAsync(uint id, UpdatePasswordDto dto);
    }
}

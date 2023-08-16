using MediCase.WebAPI.Models.Account;
using MediCase.WebAPI.Models.User;

namespace MediCase.WebAPI.Services.Interfaces
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

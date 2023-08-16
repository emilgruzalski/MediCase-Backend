using MediCase.WebAPI.Models;
using MediCase.WebAPI.Models.User;

namespace MediCase.WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<GetFullUserDto> GetByIdAsync(uint id);
        Task<GetUserDto> GetIntrospectAsync(uint id);
        Task<PagedResult<GetUserDto>> GetAllAsync(UserQuery query);
        Task<uint> CreateAsync(UserDto dto);
        Task DeleteAsync(uint id);
        Task UpdateNameAsync(uint id, UserNameDto dto);
        Task UpdatePasswordAsync(uint id, UserPasswordDto dto);
    }
}

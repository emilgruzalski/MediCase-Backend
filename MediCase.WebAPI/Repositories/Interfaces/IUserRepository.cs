using MediCase.WebAPI.Entities;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Models.User;

namespace MediCase.WebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(uint id);
        Task<User?> GetByEmailAsync(string email);
        Task<IQueryable<User>> GetAllAsync(UserQuery query);
        Task<IEnumerable<User>> GetUsersAsync();
        uint Insert(User user);
        void Delete(uint id);
        Task SaveAsync();
    }
}

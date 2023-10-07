using MediCase.API.Entities.Admin;
using MediCase.API.Models.User;
using MediCase.API.Entities;

namespace MediCase.API.Repositories.Interfaces
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

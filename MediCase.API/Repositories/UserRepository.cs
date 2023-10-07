using MediCase.API.Entities.Admin;
using MediCase.API.Models.User;
using MediCase.API.Repositories.Interfaces;
using MediCase.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCase.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MediCaseAdminContext _context;
        public UserRepository(MediCaseAdminContext context)
        {
            _context = context;
        }

        public void Delete(uint id)
        {
            User user = _context.Users.Find(id);
            _context.Users.Remove(user);
        }

        public async Task<IQueryable<User>> GetAllAsync(UserQuery query)
        {
            var baseQuery = await _context
                .Users
                .Where(r => query.SearchPhrase == null || r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower()) || r.LastName.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Email.ToLower().Contains(query.SearchPhrase.ToLower()))
                .Include(r => r.Groups).ToListAsync();

            var iBaseQuery = baseQuery.AsQueryable();

            return iBaseQuery;
        }

        public async Task<User?> GetByIdAsync(uint id)
        {
            return await _context.Users
                .Include(r => r.Groups)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.Include(r => r.Groups).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(r => r.Groups).FirstOrDefaultAsync(u => u.Email == email);
        }

        public uint Insert(User user)
        {
            _context.Users.Add(user);

            return user.Id;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

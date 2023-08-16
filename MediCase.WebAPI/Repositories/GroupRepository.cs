using MediCase.WebAPI.Entities;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Models.Group;
using MediCase.WebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCase.WebAPI.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly MediCaseAdminContext _context;

        public GroupRepository(MediCaseAdminContext context)
        {
            _context = context;
        }

        public void Delete(uint id)
        {
            Group group = _context.Groups.Find(id);
            _context.Groups.Remove(group);
        }

        public async Task<IQueryable<Group>> GetAllAsync(GroupQuery query)
        {
            var baseQuery = await _context
                .Groups
                .Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())))
                .Include(r => r.Users)
                .ToListAsync();

            var iBaseQuery = baseQuery.AsQueryable();

            return iBaseQuery;
        }

        public async Task<Group?> GetByIdAsync(uint id)
        {
            var group = await _context.Groups
                .Include (r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id);

            return group;
        }

        public uint Insert(Group group)
        {
            _context.Groups.Add(group);

            return group.Id;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

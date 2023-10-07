using MediCase.API.Entities.Admin;
using MediCase.API.Models.Group;
using MediCase.API.Entities;

namespace MediCase.API.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group?> GetByIdAsync(uint id);
        Task<IQueryable<Group>> GetAllAsync(GroupQuery query);
        uint Insert(Group group);
        void Delete(uint id);
        Task SaveAsync();
    }
}

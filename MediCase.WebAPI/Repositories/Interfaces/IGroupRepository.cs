using MediCase.WebAPI.Entities;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Models.Group;

namespace MediCase.WebAPI.Repositories.Interfaces
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

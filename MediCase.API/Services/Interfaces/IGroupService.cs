using MediCase.API.Models;
using MediCase.API.Models.Group;

namespace MediCase.API.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GetFullGroupDto> GetByIdAsync(uint id);
        Task<PagedResult<GetGroupDto>> GetAllAsync(GroupQuery query);
        Task<uint> CreateAsync(GroupDto dto);
        Task DeleteAsync(uint id);
        Task UpdateNameAsync(uint id, GroupNameDto dto);
        Task UpdateDescriptionAsync(uint id, GroupDescDto dto);
        Task UpdateExpirationDateAsync(uint id, GroupDateDto dto);
        Task UpdateRolesInGroupAsync(uint GroupId, GroupRoleDto dto);
        Task EditUsersInGroupAsync(uint GroupId, uint[] UsersId);
        Task AddUserToGroupAsync(uint GroupId, uint UserId);
        Task AddUserToGroupByMailAsync(uint GroupId, string EMail);
        Task DeleteUserToGroupAsync(uint GroupId, uint UserId);
        Task DeleteUserToGroupByMailAsync(uint GroupId, string EMail);
    }
}

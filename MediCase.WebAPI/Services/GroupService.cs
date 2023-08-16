using AutoMapper;
using MediCase.WebAPI.Entities;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Exceptions;
using MediCase.WebAPI.Models;
using MediCase.WebAPI.Models.Group;
using MediCase.WebAPI.Repositories.Interfaces;
using MediCase.WebAPI.Services.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace MediCase.WebAPI.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GroupService> _logger;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository, IMapper mapper, ILogger<GroupService> logger) 
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<uint> CreateAsync(GroupDto dto)
        {
            var group = _mapper.Map<Group>(dto);
            var result = _groupRepository.Insert(group);

/*            if (dto.IsAdmin is true)
                group.IsAdmin = true;

            if (dto.IsModerator is true)
                group.IsModerator = true;

            if (dto.IsUser is true)
                group.IsUser = true; */

            await _groupRepository.SaveAsync();

            return result;
        }

        public async Task DeleteAsync(uint id)
        {
            _logger.LogError($"Group with id: {id} DELETE action invoked");

            var group = await _groupRepository.GetByIdAsync(id);

            if (group is null)
                throw new NotFoundException($"Group not found");

            _groupRepository.Delete(id);
            await _groupRepository.SaveAsync();
        }

        public async Task<PagedResult<GetGroupDto>> GetAllAsync(GroupQuery query)
        {
            var baseQuery = await _groupRepository.GetAllAsync(query);

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Group, object>>>
                {
                    { nameof(Group.Name), r => r.Name },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var groups = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var groupDtos = _mapper.Map<List<GetGroupDto>>(groups);

            var result = new PagedResult<GetGroupDto>(groupDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public async Task<GetFullGroupDto> GetByIdAsync(uint id)
        {
            var group = await _groupRepository.GetByIdAsync(id);

            if (group is null)
                throw new NotFoundException("Group not found");

            var result = _mapper.Map<GetFullGroupDto>(group);

            return result;
        }

        public async Task UpdateNameAsync(uint id, GroupNameDto dto)
        {
            var group = await _groupRepository.GetByIdAsync(id);

            if (group is null)
                throw new NotFoundException("Group not found");

            group.Name = dto.Name;
            
            await _groupRepository.SaveAsync();
        }

        public async Task UpdateDescriptionAsync(uint id, GroupDescDto dto)
        {
            var group = await _groupRepository.GetByIdAsync(id);

            if (group is null)
                throw new NotFoundException("Group not found");

            group.Description = dto.Description;

            await _groupRepository.SaveAsync();
        }

        public async Task UpdateExpirationDateAsync(uint id, GroupDateDto dto)
        {
            var group = await _groupRepository.GetByIdAsync(id);

            if (group is null)
                throw new NotFoundException("Group not found");

            group.ExpirationDate = dto.ExpirationDate;

            await _groupRepository.SaveAsync();
        }

        public async Task UpdateRolesInGroupAsync(uint GroupId, GroupRoleDto dto) 
        {
            var group = await _groupRepository.GetByIdAsync(GroupId);

            if (group is null)
                throw new NotFoundException("Group not found");

            group.IsAdmin = dto.IsAdmin;
            group.IsModerator = dto.IsModerator;
            group.IsUser = dto.IsUser;

            await _groupRepository.SaveAsync();
        }

        public async Task EditUsersInGroupAsync(uint GroupId, uint[] UsersId)
        {
            var group = await _groupRepository.GetByIdAsync(GroupId);

            if (group is null)
                throw new NotFoundException("Group not found");

            var users = group.Users.Select(p => p.Id).ToArray();

            var usersToAdd = UsersId.Except(users).ToArray();
            var usersToRemove = users.Except(UsersId).ToArray();

            foreach (var userId in usersToAdd)
            {
                group.Users.Add(new User() { Id = userId });
            }

            foreach (var userId in usersToRemove)
            {
                var groupUser = group.Users.FirstOrDefault(p => p.Id == userId);
                if (groupUser != null)
                    group.Users.Remove(groupUser);
            }

            await _groupRepository.SaveAsync();
        }

        public async Task AddUserToGroupAsync(uint GroupId, uint UserId)
        {
            var group = await _groupRepository.GetByIdAsync(GroupId);

            if (group is null)
                throw new NotFoundException("Group not found");

            var user = await _userRepository.GetByIdAsync(UserId);

            if (user is null)
                throw new NotFoundException("User not found");

            group.Users.Add(new User() { Id = UserId });

            await _groupRepository.SaveAsync();
        }

        public async Task AddUserToGroupByMailAsync(uint GroupId, string EMail)
        {
            var group = await _groupRepository.GetByIdAsync(GroupId);

            if (group is null)
                throw new NotFoundException("Group not found");

            var user = await _userRepository.GetByEmailAsync(EMail);
            if (user is null)
                throw new NotFoundException("User not found");

            group.Users.Add(new User() { Id = user.Id });

            await _groupRepository.SaveAsync();
        }

        public async Task DeleteUserToGroupAsync(uint GroupId, uint UserId)
        {
            var group = await _groupRepository.GetByIdAsync(GroupId);

            if (group is null)
                throw new NotFoundException("Group not found");

            var user = await _userRepository.GetByIdAsync(UserId);

            if (user is null)
                throw new NotFoundException("User not found");

            var groupUser = group.Users.FirstOrDefault(p => p.Id == UserId);

            if (groupUser != null)
                group.Users.Remove(groupUser);

            await _groupRepository.SaveAsync();
        }

        public async Task DeleteUserToGroupByMailAsync(uint GroupId, string EMail)
        {
            var group = await _groupRepository.GetByIdAsync(GroupId);
            if(group is null)
                throw new NotFoundException("Group not found");

            var user = await _userRepository.GetByEmailAsync(EMail);

            if (user is null)
                throw new NotFoundException("User not found");

            var groupUser = group.Users.FirstOrDefault(p => p.Id == user.Id);

            if(groupUser != null)
                group.Users.Remove(groupUser);

            await _groupRepository.SaveAsync();
        }
    }
}

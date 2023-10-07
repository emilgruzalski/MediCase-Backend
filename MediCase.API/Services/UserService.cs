using AutoMapper;
using MediCase.API.Entities.Admin;
using MediCase.API.Models;
using MediCase.API.Models.User;
using MediCase.API.Repositories.Interfaces;
using MediCase.API.Services.Interfaces;
using MediCase.API.Entities;
using MediCase.API.Exceptions;
using MediCase.API.Repositories;
using System.Linq.Expressions;

namespace MediCase.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<uint> CreateAsync(UserDto dto)
        {
            var newUser = _mapper.Map<User>(dto);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            newUser.PasswordHash = hashedPassword;
            var result = _userRepository.Insert(newUser);
            await _userRepository.SaveAsync();
            return result;
        }

        public async Task DeleteAsync(uint id)
        {
            _logger.LogError($"User with id: {id} DELETE action invoked");

            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException($"User not found");

            _userRepository.Delete(id);
            await _userRepository.SaveAsync();
        }

        public async Task<PagedResult<GetUserDto>> GetAllAsync(UserQuery query)
        {
            var baseQuery = await _userRepository.GetAllAsync(query);

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<User, object>>>
                {
                    { nameof(User.FirstName), r => r.FirstName },
                    { nameof(User.LastName), r => r.LastName },
                    { nameof(User.Email), r => r.Email },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var users = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var userDtos = _mapper.Map<List<GetUserDto>>(users);

            var result = new PagedResult<GetUserDto>(userDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public async Task<GetFullUserDto> GetByIdAsync(uint id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found");

            var result = _mapper.Map<GetFullUserDto>(user);
            return result;
        }

        public async Task<GetUserDto> GetIntrospectAsync(uint id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found");

            var result = _mapper.Map<GetUserDto>(user);
            return result;
        }

        public async Task UpdateNameAsync(uint id, UserNameDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            await _userRepository.SaveAsync();
        }

        public async Task UpdatePasswordAsync(uint id, UserPasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            user.PasswordHash = hashedPassword;
            await _userRepository.SaveAsync();
        }
    }
}

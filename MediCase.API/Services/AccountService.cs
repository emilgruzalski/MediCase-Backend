using AutoMapper;
using BCrypt.Net;
using MediCase.API.Entities.Admin;
using MediCase.API.Models.Account;
using MediCase.API.Models.User;
using MediCase.API.Repositories.Interfaces;
using MediCase.API.Services.Interfaces;
using MediCase.API.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediCase.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(IUserRepository userRepository, IMapper mapper, ILogger<AccountService> logger, AuthenticationSettings authenticationSettings)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _authenticationSettings = authenticationSettings;
        }

        public async Task<string> GenerateJwtAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var result = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (result == false)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var userDto = _mapper.Map<GetUserDto>(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Email, $"{user.Email}")
            };

            foreach (var role in userDto.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public async Task RegisterUserAsync(UserDto dto)
        {
            var newUser = _mapper.Map<User>(dto);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            newUser.PasswordHash = hashedPassword;
            _userRepository.Insert(newUser);
            await _userRepository.SaveAsync();
        }

        public async Task UpdateEmailAsync(uint id, UpdateEmailDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }

            var result = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (result == false)
            {
                throw new BadRequestException("Invalid password");
            }

            if (user.Email == dto.Email)
            {
                throw new BadRequestException("Email is identical to actual one");
            }

            var userPool = await _userRepository.GetUsersAsync();

            if (userPool.Any(p => p.Email == dto.Email))
            {
                throw new BadRequestException("Email is already taken");
            }

            user.Email = dto.Email;

            await _userRepository.SaveAsync();
        }

        public async Task UpdateNameAsync(uint id, UpdateNameDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            await _userRepository.SaveAsync();
        }

        public async Task UpdatePasswordAsync(uint id, UpdatePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }

            var result = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash);

            if (result == false)
            {
                throw new BadRequestException("Invalid password");
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                throw new BadRequestException("Passwords don't match");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.PasswordHash = hashedPassword;

            await _userRepository.SaveAsync();
        }
    }
}

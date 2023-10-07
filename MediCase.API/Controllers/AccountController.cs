using MediCase.API.Models.Account;
using MediCase.API.Models.User;
using MediCase.API.Services.Interfaces;
using MediCase.API.Entities.Admin;
using MediCase.API.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MediCase.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly JwtBearerHandler _tokenHandler;

        public AccountController(IAccountService accountService, IUserService userService, IHttpContextAccessor contextAccessor, JwtBearerHandler tokenHandler)
        {
            _accountService = accountService;
            _userService = userService;
            _contextAccessor = contextAccessor;
            _tokenHandler = tokenHandler;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUserAsync([FromBody] UserDto dto)
        {
            await _accountService.RegisterUserAsync(dto);

            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            string token = await _accountService.GenerateJwtAsync(dto);

            return Ok(token);
        }

        [HttpDelete("logout")]
        public ActionResult LogoutAsync()
        {
            return Ok();
        }

        [HttpPut("password")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] UpdatePasswordDto dto)
        {
            var userIdString = _contextAccessor.HttpContext.User.Identities.FirstOrDefault().FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = uint.Parse(userIdString);

            await _accountService.UpdatePasswordAsync(userId, dto);

            return Ok();
        }

        [HttpPut("email")]
        public async Task<ActionResult> ChangeEmailAsync([FromBody] UpdateEmailDto dto)
        {
            var userIdString = _contextAccessor.HttpContext.User.Identities.FirstOrDefault().FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = uint.Parse(userIdString);

            await _accountService.UpdateEmailAsync(userId, dto);

            return Ok();
        }

        [HttpPut("detalis")]
        public async Task<ActionResult> EditDetailsAsync([FromBody] UpdateNameDto dto)
        {
            var userIdString = _contextAccessor.HttpContext.User.Identities.FirstOrDefault().FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = uint.Parse(userIdString);

            await _accountService.UpdateNameAsync(userId, dto);

            return Ok();
        }

        [HttpGet("introspect")]
        public async Task<ActionResult<GetUserDto>> Introspect()
        {
            var userIdString = _contextAccessor.HttpContext.User.Identities.FirstOrDefault().FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = uint.Parse(userIdString);

            var user = await _userService.GetIntrospectAsync(userId);

            return Ok(user);
        }
    }
}

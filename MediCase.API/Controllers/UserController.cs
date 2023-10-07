using MediCase.API.Models.User;
using MediCase.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserDto dto)
        {
            var id = await _userService.CreateAsync(dto);

            return Created($"/user/{id}", null);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll([FromQuery] UserQuery query)
        {
            var usersDtos = await _userService.GetAllAsync(query);

            return Ok(usersDtos);
        }

        [HttpPut("{id}/name")]
        public async Task<ActionResult> UpdateName([FromBody] UserNameDto dto, [FromRoute] uint id)
        {
            await _userService.UpdateNameAsync(id, dto);

            return Ok();
        }

        [HttpPut("{id}/password")]
        public async Task<ActionResult> UpdatePassword([FromBody] UserPasswordDto dto, [FromRoute] uint id)
        {
            await _userService.UpdatePasswordAsync(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] uint id)
        {
            await _userService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetFullUserDto>> GetById([FromRoute] uint id)
        {
            var userDto = await _userService.GetByIdAsync(id);

            return Ok(userDto);
        }
    }
}

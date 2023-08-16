using MediCase.WebAPI.Models.Group;
using MediCase.WebAPI.Services;
using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] GroupDto dto)
        {
            var id = await _groupService.CreateAsync(dto);

            return Created($"/user/{id}", null);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetGroupDto>>> GetAll([FromQuery] GroupQuery query)
        {
            var usersDtos = await _groupService.GetAllAsync(query);

            return Ok(usersDtos);
        }

        [HttpPut("{id}/name")]
        public async Task<ActionResult> UpdateName([FromBody] GroupNameDto dto, [FromRoute] uint id)
        {
            await _groupService.UpdateNameAsync(id, dto);

            return Ok();
        }

        [HttpPut("{id}/description")]
        public async Task<ActionResult> UpdateDescription([FromBody] GroupDescDto dto, [FromRoute] uint id)
        {
            await _groupService.UpdateDescriptionAsync(id, dto);

            return Ok();
        }

        [HttpPut("{id}/expirationDate")]
        public async Task<ActionResult> UpdateExpirationDate([FromBody] GroupDateDto dto, [FromRoute] uint id)
        {
            await _groupService.UpdateExpirationDateAsync(id, dto);

            return Ok();
        }

        [HttpPut("{id}/roles")]
        public async Task<ActionResult> UpdateRolesInGroup([FromBody] GroupRoleDto dto, [FromRoute] uint id)
        {
            await _groupService.UpdateRolesInGroupAsync(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] uint id)
        {
            await _groupService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetFullGroupDto>> Get([FromRoute] uint id)
        {
            var userDto = await _groupService.GetByIdAsync(id);

            return Ok(userDto);
        }

        [HttpPut("{id}/users")]
        public async Task<ActionResult> EditUsersInGroup([FromRoute] uint id, [FromBody] uint[] usersId)
        {
            await _groupService.EditUsersInGroupAsync(id, usersId);

            return Ok();
        }

        [HttpPost("{id}/users")]
        public async Task<ActionResult> AddUserToGroup([FromRoute] uint id, [FromBody] uint userId)
        {
            await _groupService.AddUserToGroupAsync(id, userId);

            return Ok();
        }

        [HttpPost("{id}/user/manage")]
        public async Task<ActionResult> AddUserToGroupByEMail([FromRoute] uint id, [FromBody] string email)
        {
            await _groupService.AddUserToGroupByMailAsync(id, email);
            return Ok();
        }

        [HttpDelete("{id}/users")]
        public async Task<ActionResult> DeleteUserToGroup([FromRoute] uint id, [FromQuery] uint userId)
        {
            await _groupService.DeleteUserToGroupAsync(id, userId);

            return Ok();
        }

        [HttpDelete("{id}/user/manage")]
        public async Task<ActionResult> DeleteUserToGroupByEmail([FromRoute] uint id, [FromQuery] string email)
        {
            await _groupService.DeleteUserToGroupByMailAsync(id, email);

            return Ok();
        }
    }
}

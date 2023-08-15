using MediCase.WebAPI.Models.Entity;
using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.WebAPI.Controllers.Moderator
{
    [Route("api/Moderator/Entity/[controller]")]
    [ApiController]
    public class EntityTypesController : Controller
    {
        private readonly IModEntityService _entityService;

        public EntityTypesController(IModEntityService entityService)
        {
            _entityService = entityService;

        }

        [HttpGet("getTypes")]
        public async Task<IActionResult> GetEntityTypes()
        {
            return Ok(await _entityService.GetEntityTypesAsync());
        }

        [HttpPatch("updateType")]
        public async Task<IActionResult> UpdateEntityType([FromBody] EntityTypeDto entityTypeDTO)
        {
            await _entityService.UpdateEntityTypeAsync(entityTypeDTO);
            return Ok();
        }

        [HttpPost("addType")]
        public async Task<IActionResult> AddEntityType([FromQuery] string entityType)
        {
            return Ok(await _entityService.AddEntityTypeAsync(entityType));
        }

        [HttpDelete("deleteType")]
        public async Task<IActionResult> DeleteEntityType([FromQuery] uint typeId)
        {
            await _entityService.DeleteEntityTypeAsync(typeId);
            return Ok();
        }

    }
}

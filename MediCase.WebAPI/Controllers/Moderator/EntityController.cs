using MediCase.WebAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MediCase.WebAPI.Controllers.Moderator
{
    [Route("api/Moderator/[controller]")]
    [ApiController]
    public class EntityController : Controller
    {
        private readonly IModEntityService _entityService;
        private readonly ILogger _logger;

        public EntityController(IModEntityService entityService, ILogger<EntityController> logger)
        {
            _entityService = entityService;
            _logger = logger;
        }

        [HttpPatch("refreshEntityLock")]
        public async Task<IActionResult> RefreshEntityLockAsync([FromQuery] uint entityId, [FromQuery] uint seconds)
        { 
            await _entityService.RefreshEntityLockAsync(entityId, seconds);
            return Ok();
           
        }

        [HttpGet("isEntityLocked")]
        public async Task<IActionResult> IsEntityLocked([FromQuery] uint entityId)
        {
            return Ok(await _entityService.IsEntityLockedAsync(entityId));
        }

        [HttpPost("addEntity")]
        public async Task<IActionResult> AddEntity([FromQuery] uint? parentId, [FromQuery] EntityPostDto newEntity)
        {
            return Ok(await _entityService.AddEntityAsync(parentId, newEntity));
        }

        [HttpDelete("deleteEntity")]
        public async Task<IActionResult> DeleteEntity([FromQuery] uint entityId)
        {
            await _entityService.DeleteEntityAsync(entityId);
            return Ok();
        }

        [HttpPatch("updateEntityOrder")]
        public async Task<IActionResult> UpdateEntityOrder([FromBody] List<EntityUpdateDto> entityDTOs)
        {
            await _entityService.UpdateEntityOrderAsync(entityDTOs);
            return Ok();
        }

        [HttpGet("getEntities")]
        public async Task<ActionResult> GetEntities()
        {
            return Ok(await _entityService.GetEntitiesAsync());
        }

        [HttpGet("getEntitiesWithTranslations")]
        public async Task<IActionResult> GetEntitiesWithTranslations()
        {
            return Ok(await _entityService.GetEntitiesWithTranslationsAsync());
        }

        [HttpGet("getEntityWithTranslations")]
        public async Task<IActionResult> GetEntityWithTranslations([FromQuery] uint entityId)
        {
            return Ok(await _entityService.GetEntityWithTranslationsAsync(entityId));   
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCase.API.Services.Interfaces;

namespace MediCase.API.Controllers.Content
{
    [Route("api/Main/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class EntityController : Controller
    {
        private readonly IEntityService _entityService;

        public EntityController(IEntityService entityService)
        {
            _entityService = entityService;

        }

        [HttpGet("getEntities")]
        public async Task<ActionResult> GetEntities()
        {
            var entities = await _entityService.GetEntitiesAsync();
            return Ok(entities);
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

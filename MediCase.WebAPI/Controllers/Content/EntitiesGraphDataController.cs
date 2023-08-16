using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.WebAPI.Controllers.Content
{
    [Route("api/Main/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class EntitiesGraphDataController : Controller
    {
        private readonly IEntityService _entityService;

        public EntitiesGraphDataController(IEntityService entityService)
        {
            _entityService = entityService;

        }
        [HttpGet("getEntityWithChilds")]
        public async Task<IActionResult> GetEntityWithChilds([FromQuery] uint? entityId)
        {
           return Ok(await _entityService.GetEntityWithChildsAsync(entityId));
        }

        [HttpGet("getEntityChildsByLanguage")]
        public async Task<IActionResult> GetEntityChildsByLanguage([FromQuery] uint? entityId, [FromQuery] uint[] langIDs)
        {
            return Ok(await _entityService.GetEntityChildsByLanguageAsync(entityId, langIDs));
        }

        [HttpGet("getEntityWithChildsByLanguage")]
        public async Task<IActionResult> GetEntityWithChildsByLanguage([FromQuery] uint? entityId, [FromQuery] uint[] langIDs)
        {
            return Ok(await _entityService.GetEntityWithChildsByLanguageAsync(entityId, langIDs));
        }
    }
}

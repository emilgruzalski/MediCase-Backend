using MediCase.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.API.Controllers.Moderator
{
    [Route("api/Moderator/[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator,Admin")]
    public class EntitiesGraphDataController : Controller
    {
        private readonly IModEntityService _entityService;

        public EntitiesGraphDataController(IModEntityService entityService)
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

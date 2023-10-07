using MediCase.API.Models.Entity;
using MediCase.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.API.Controllers.Moderator
{
    [Route("api/Moderator/Entity/[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator")]
    public class EntityLanguagesController : Controller
    {
        private readonly IModEntityService _entityService;

        public EntityLanguagesController(IModEntityService entityService)
        {
            _entityService = entityService;
        }

        [AllowAnonymous]
        [HttpGet("getLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            return Ok(await _entityService.GetLanguagesAsync());
        }

        [HttpPost("addLanguage")]
        public async Task<IActionResult> AddLanguage([FromQuery] string langCode)
        {
            return Ok(await _entityService.AddLanguageAsync(langCode));
        }

        [HttpDelete("deleteLanguage")]
        public async Task<IActionResult> DeleteLanguage([FromQuery] uint langId)
        {
            await _entityService.DeleteLanguageAsync(langId);
            return Ok();
        }

        [HttpPatch("updateLanguage")]
        public async Task<IActionResult> UpdateLanguage([FromBody] EntityLanguageUpdateDto entityLanguage)
        {
            await _entityService.UpdateLanguageAsync(entityLanguage);
            return Ok();
        }
    }
}

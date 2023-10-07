using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Authorization;
using MediCase.API.Services.Interfaces;
using MediCase.API.Models.Entity;

namespace MediCase.API.Controllers.Moderator
{
    [Route("api/Moderator/[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator")]
    public class EntityTranslationsController : Controller
    {
        private readonly IModEntityService _entityService;
        private readonly ITranslationGeneratorService _translationGeneratorService;
        public EntityTranslationsController(IModEntityService entityService, ITranslationGeneratorService translationGeneratorService,
            IImageGeneratorService imageGeneratorService)
        {
            _entityService = entityService;
            _translationGeneratorService = translationGeneratorService;
        }

        [HttpPost("addEntityTranslation")]
        public async Task<IActionResult> AddEntityTranslation([FromBody] EntityTranslationPostDto newEntityTranslation)
        {
            return Ok(await _entityService.AddEntityTranslationAsync(newEntityTranslation));
        }

        [HttpPost("generateEntityTranslation")]
        public async Task<IActionResult> GenerateEntityTranslations([FromQuery] uint translationId, [FromQuery] uint desiredLanguageId)
        {
            return Ok(await _translationGeneratorService.GenerateTranslationAsync(translationId, desiredLanguageId));
        }

        [HttpDelete("deleteEntityTranslation")]
        public async Task<IActionResult> DeleteEntityTranslation([FromQuery] uint entityTranslationId)
        {
            await _entityService.DeleteEntityTranslationAsync(entityTranslationId);
            return Ok();
        }

        [HttpPatch("updateEntityTranslation")]
        public async Task<IActionResult> UpdateEntityTranslation([FromBody] EntityTranslationUpdateDto entityTranslationDTO)
        {
            await _entityService.UpdateEntityTranslationAsync(entityTranslationDTO);
            return Ok();
        }

        [Authorize(Roles = "Moderator,Admin")]
        [HttpGet("getEntityTranslations")]
        public async Task<IActionResult> GetEntityTranslations()
        {
            return Ok(await _entityService.GetEntityTranslationsAsync());
        }

    }
}

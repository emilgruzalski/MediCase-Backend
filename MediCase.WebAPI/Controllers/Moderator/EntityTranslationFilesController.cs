using Microsoft.AspNetCore.Mvc;
using MediCase.WebAPI.Models.Entity;
using MediCase.WebAPI.Services.Interfaces;
using System.Net.Mime;
using MediCase.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace MediCase.WebAPI.Controllers.Moderator
{
    [Route("api/Moderator/[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator")]
    public class EntityTranslationFilesController : Controller
    {
        private readonly IModEntityService _entityService;
        private readonly IImageGeneratorService _imageGeneratorService;
        private readonly IVoiceGeneratorService _voiceGeneratorService;

        public EntityTranslationFilesController(IModEntityService entityService, 
            IImageGeneratorService imageGeneratorService,
            IVoiceGeneratorService voiceGeneratorService)
        {
            _entityService = entityService;
            _imageGeneratorService = imageGeneratorService;
            _voiceGeneratorService = voiceGeneratorService;
        }

        [HttpPost("generateEntityTranslationVoice")]
        public async Task<IActionResult> GenerateEntityTranslationVoice([FromQuery] uint translationId, [FromQuery] string? referredField)
        {
            
            await _voiceGeneratorService.GenerateVoiceAsync(translationId, referredField);
            return Ok();

        }

        [HttpGet("getSupportedTranslationVoices")]
        public async Task<IActionResult> GetSupportedTranslationVoices()
        {
            return Ok(await _voiceGeneratorService.GetSupportedLanguagesAsync());
            
        }

        [HttpPost("addEntityTranslationFile")]
        public async Task<IActionResult> AddEntityTranslationFile([FromForm] EntityTranslationFilePostDto newEntityTranslationFile)
        {
            return Ok(await _entityService.AddEntityTranslationFileAsync(newEntityTranslationFile));
        }

        [Authorize(Roles = "Moderator,Admin")]
        [HttpGet("getEntityTranslationFile")]
        public async Task<IActionResult> GetEntityTranslationFile([FromQuery] string fileIdentifier)
        {
            return File(await _entityService.GetEntityTranslationFileAsync(fileIdentifier), MediaTypeNames.Text.Plain);
        }

        [HttpDelete("deleteEntityTranslationFile")]
        public async Task<IActionResult> DeleteEntityTranslationFile([FromQuery] uint entityTranslationFileId)
        {
            await _entityService.DeleteEntityTranslationFileAsync(entityTranslationFileId);
            return Ok();
        }


    }
}

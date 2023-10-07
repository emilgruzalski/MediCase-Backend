using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using MediCase.API.Services.Interfaces;

namespace MediCase.API.Controllers.Content
{
    [Route("api/Main/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class EntityTranslationFilesController : Controller
    {
        private readonly IEntityService _entityService;

        public EntityTranslationFilesController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [AllowAnonymous]
        [HttpGet("getEntityTranslationFile")]
        public async Task<IActionResult> GetEntityTranslationFile([FromQuery] string fileIdentifier)
        {
            return File(await _entityService.GetEntityTranslationFileAsync(fileIdentifier), MediaTypeNames.Text.Plain);
        }


    }
}

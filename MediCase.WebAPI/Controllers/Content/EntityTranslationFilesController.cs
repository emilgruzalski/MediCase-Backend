using Microsoft.AspNetCore.Mvc;
using MediCase.WebAPI.Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;

namespace MediCase.WebAPI.Controllers.Content
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

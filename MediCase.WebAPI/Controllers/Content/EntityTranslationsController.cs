using Microsoft.AspNetCore.Mvc;
using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MediCase.WebAPI.Controllers.Content
{
    [Route("api/Main/[controller]")]
    [ApiController]
    public class EntityTranslationsController : Controller
    {
        private readonly IEntityService _entityService;

        public EntityTranslationsController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [HttpGet("getEntityTranslations")]
        public async Task<IActionResult> GetEntityTranslations()
        {
            return Ok(await _entityService.GetEntityTranslationsAsync());
        }

    }
}

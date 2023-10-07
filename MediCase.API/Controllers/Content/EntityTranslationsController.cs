using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCase.API.Services.Interfaces;

namespace MediCase.API.Controllers.Content
{
    [Route("api/Main/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
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

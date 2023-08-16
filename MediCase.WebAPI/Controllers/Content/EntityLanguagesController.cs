using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.WebAPI.Controllers.Content
{
    [Route("api/Main/Entity/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class EntityLanguagesController : Controller
    {
        private readonly IEntityService _entityService;

        public EntityLanguagesController(IEntityService entityService)
        {
            _entityService = entityService;

        }

        [AllowAnonymous]
        [HttpGet("getLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            return Ok(await _entityService.GetLanguagesAsync());
        }

    }
}

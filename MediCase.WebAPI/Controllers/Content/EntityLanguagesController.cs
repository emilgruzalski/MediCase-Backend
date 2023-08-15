using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.WebAPI.Controllers.Content
{
    [Route("api/Main/Entity/[controller]")]
    [ApiController]
    public class EntityLanguagesController : Controller
    {
        private readonly IEntityService _entityService;

        public EntityLanguagesController(IEntityService entityService)
        {
            _entityService = entityService;

        }


        [HttpGet("getLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            return Ok(await _entityService.GetLanguagesAsync());
        }

    }
}

using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.WebAPI.Controllers.Content
{
    [Route("api/Main/Entity/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class EntityTypesController : Controller
    {
        private readonly IEntityService _entityService;

        public EntityTypesController(IEntityService entityService)
        {
            _entityService = entityService;

        }

        [AllowAnonymous]
        [HttpGet("getTypes")]
        public async Task<IActionResult> GetEntityTypes()
        {
            return Ok(await _entityService.GetEntityTypesAsync());
        }

    }
}

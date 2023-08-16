using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCase.WebAPI.Controllers.Moderator
{
    [Route("api/Moderator/Entity/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ModeratorBucketController : Controller
    {
        private readonly IModEntityService _entityService;

        public ModeratorBucketController(IModEntityService entityService)
        {
            _entityService = entityService;

        }

        [HttpGet("checkState")]
        public async Task<IActionResult> GetBucketState()
        {
            return Ok(await _entityService.CheckIfWeHaveChangesAsync());
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MediCase.WebAPI.Controllers.Content
{
    [Route("api/Main/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EntitySynchronizationController : Controller
    {
        private readonly ISynchronizationService _syncService;


        public EntitySynchronizationController(ISynchronizationService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost("syncDatabases")]
        public async Task<IActionResult> SyncDatabases()
        {
            await _syncService.SynchronizeDatabasesAsync();
            return Ok();
        }

    }
}

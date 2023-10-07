using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCase.API.Services.Interfaces;

namespace MediCase.API.Controllers.Content
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

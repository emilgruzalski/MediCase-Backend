using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Exceptions;
using MediCase.API.Entities.Content;
using MediCase.API.Repositories.Content.Interfaces;

namespace MediCase.API.Repositories.Content
{
    public class EntityRepository : IEntityRepository
    {
        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;

        public EntityRepository(MediCaseContentContext context, ILogger<EntityRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Entity>> GetEntitiesAsync()
        {
            return await _context.Entities.OrderBy(x => x.EntityOrder).ToListAsync();
        }

        public async Task<Entity> GetEntityWithTranslationsAsync(uint entityId)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).Include(add => add.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException("Entity not found");
            return entity;
        }
        public async Task<List<Entity>> GetEntitiesWithTranslationsAsync()
        {
            return await _context.Entities.Include(add => add.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles).ToListAsync();
        }
    }
}

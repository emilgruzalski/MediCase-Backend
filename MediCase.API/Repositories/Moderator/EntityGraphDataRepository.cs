using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Exceptions;
using MediCase.API.Entities.Moderator;
using MediCase.API.Repositories.Moderator.Interfaces;

namespace MediCase.API.Repositories.Moderator
{
    public class EntityGraphDataRepository : IEntityGraphDataRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;

        public EntityGraphDataRepository(MediCaseModeratorContext context, ILogger<EntityGraphDataRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Entity> GetEntityWithChildsAsync(uint? entityId)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).Include(x => x.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles)
                .Include(x => x.EntitiesGraphDatumParents).ThenInclude(x => x.Child).ThenInclude(x => x.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException($"Entity not found: {entityId}");
            return entity;
        }
        public async Task<Entity> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).Include(x => x.EntityTranslations.Where(x => langIDs.Contains(x.LangId))).ThenInclude(x => x.EntityTranslationFiles)
                .Include(x => x.EntitiesGraphDatumParents).ThenInclude(x => x.Child).ThenInclude(x => x.EntityTranslations.Where(x => langIDs.Contains(x.LangId))).ThenInclude(x => x.EntityTranslationFiles).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException($"Entity not found: {entityId}");
            return entity;
        }

        public async Task<List<EntitiesGraphDatum>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs)
        {
            var entities = await _context.EntitiesGraphData.Where(x => x.ParentId == entityId)
                .Include(x => x.Child).ThenInclude(x => x.EntityTranslations.Where(x => langIDs.Contains(x.LangId))).ThenInclude(x => x.EntityTranslationFiles).ToListAsync();
            return entities;
        }

        public async Task<EntitiesGraphDatum> AttachEntityToParentAsync(EntitiesGraphDatum entityRelation)
        {
            _context.EntitiesGraphData.Add(entityRelation);
            await _context.SaveChangesAsync();
            _context.Entry(entityRelation).State = EntityState.Detached;
            return entityRelation;
        }
    }
}

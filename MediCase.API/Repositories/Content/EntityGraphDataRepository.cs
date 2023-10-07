using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Exceptions;
using MediCase.API.Entities.Content;
using MediCase.API.Repositories.Content.Interfaces;

namespace MediCase.API.Repositories.Content
{
    public class EntityGraphDataRepository : IEntityGraphDataRepository
    {
        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;

        public EntityGraphDataRepository(MediCaseContentContext context, ILogger<EntityGraphDataRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Entity> GetEntityWithChildsAsync(uint? entityId)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).Include(x => x.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles)
                .Include(x => x.EntitiesGraphDatumParents).ThenInclude(x => x.Child).ThenInclude(x => x.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException("Entity not found");
            return entity;
        }
        public async Task<Entity> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).Include(x => x.EntityTranslations.Where(x => langIDs.Contains(x.LangId))).ThenInclude(x => x.EntityTranslationFiles)
                .Include(x => x.EntitiesGraphDatumParents).ThenInclude(x => x.Child).ThenInclude(x => x.EntityTranslations.Where(x => langIDs.Contains(x.LangId))).ThenInclude(x => x.EntityTranslationFiles).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException("Entity not found");
            return entity;
        }

        public async Task<List<EntitiesGraphDatum>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs)
        {
            return await _context.EntitiesGraphData.Where(x => x.ParentId == entityId)
                .Include(x => x.Child).ThenInclude(x => x.EntityTranslations.Where(x => langIDs.Contains(x.LangId))).ThenInclude(x => x.EntityTranslationFiles).ToListAsync();
        }

    }
}

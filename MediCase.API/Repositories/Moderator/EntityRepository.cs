using MediCase.API.Entities.Moderator;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCase.API.Repositories.Moderator
{
    public class EntityRepository : IEntityRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;

        public EntityRepository(MediCaseModeratorContext context, ILogger<EntityRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task RefreshEntityLockAsync(uint entityId, uint newTime)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException($"Entity not found: {entityId}");
            entity.LockExpirationDate = DateTime.UtcNow.AddSeconds(newTime);
            await _context.SaveChangesAsync();
        }

        public async Task<Entity> AddEntityAsync(Entity entity)
        {
            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task DeleteEntityAsync(uint entityId)
        {
            var isChild = _context.EntitiesGraphData.Where(x => x.ChildId == entityId).Any();
            var isParent = _context.EntitiesGraphData.Where(x => x.ParentId == entityId).Any();
            if (isChild || isParent)
            {
                _context.Database.ExecuteSqlRaw("call DeleteNode(" + entityId.ToString() + ");");
            }
            else
            {
                var retrivedObject = _context.Entities.Where(x => x.EntityId == entityId).FirstOrDefault();
                if (retrivedObject == null) throw new NotFoundException($"Entity not found: {entityId}");
                _context.Entities.Remove(retrivedObject);
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateEntityOrderAsync(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entities.Update(entity);
                _context.Entry(entity).Property(x => x.TypeId).IsModified = false;

            }
            await _context.SaveChangesAsync();
        }
        public async Task<List<Entity>> GetEntitiesAsync()
        {
            return await _context.Entities.OrderBy(x => x.EntityOrder).ToListAsync();
        }

        public async Task<Entity> GetEntityWithTranslationsAsync(uint entityId)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).Include(add => add.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException($"Entity not found: {entityId}");
            return entity;
        }
        public async Task<List<Entity>> GetEntitiesWithTranslationsAsync()
        {
            return await _context.Entities.Include(add => add.EntityTranslations).ThenInclude(x => x.EntityTranslationFiles).ToListAsync();
        }

        public async Task<Entity> MarkEntityHasChilds(uint? entityId)
        {
            var entity = _context.Entities.Where(x => x.EntityId == entityId).FirstOrDefault();
            if (entity == null) throw new NotFoundException($"Entity not found: {entityId}");
            entity.HasChilds = true;
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<bool> IsEntityLockedAsync(uint entityId)
        {
            var entity = await _context.Entities.Where(x => x.EntityId == entityId).FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException($"Entity not found: {entityId}");
            return entity.LockExpirationDate <= DateTime.UtcNow ? false : true;
        }
    }
}


using MediCase.WebAPI.Entities.Moderator;
using MediCase.WebAPI.Repositories.Moderator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.WebAPI.Exceptions;

namespace MediCase.WebAPI.Repositories.Moderator
{
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;

        public EntityTypeRepository(MediCaseModeratorContext context, ILogger<EntityTypeRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<EntityType>> GetEntityTypesAsync()
        {
            return await _context.EntityTypes.ToListAsync();
        }

        public async Task UpdateEntityTypeAsync(EntityType entityType)
        {
            _context.EntityTypes.Update(entityType);
            await _context.SaveChangesAsync();
        }

        public async Task<EntityType> AddEntityTypeAsync(EntityType newEntityType)
        {
            _context.EntityTypes.Add(newEntityType);
            await _context.SaveChangesAsync();
            _context.Entry(newEntityType).State = EntityState.Detached;
            return newEntityType;
        }

        public async Task DeleteEntityTypeAsync(uint typeId)
        {
            var item = _context.EntityTypes.Where(x => x.TypeId == typeId).FirstOrDefault();
            if (item == null) throw new NotFoundException($"Type not found: {typeId}");
            _context.EntityTypes.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}

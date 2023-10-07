using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Entities.Content;
using MediCase.API.Repositories.Content.Interfaces;

namespace MediCase.API.Repositories.Content
{
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;

        public EntityTypeRepository(MediCaseContentContext context, ILogger<EntityTypeRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<EntityType>> GetEntityTypesAsync()
        {
            return await _context.EntityTypes.ToListAsync();
        }

    }
}

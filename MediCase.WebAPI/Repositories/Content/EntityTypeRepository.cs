using MediCase.WebAPI.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using MediCase.WebAPI.Repositories.Content.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediCase.WebAPI.Repositories.Content
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

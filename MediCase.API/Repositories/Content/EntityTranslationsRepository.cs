using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Repositories.Content.Interfaces;
using MediCase.API.Entities.Content;

namespace MediCase.API.Repositories.Content
{
    public class EntityTranslationsRepository : IEntityTranslationsRepository
    {
        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;

        public EntityTranslationsRepository(MediCaseContentContext context, ILogger<EntityTranslationsRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<EntityTranslation>> GetEntityTranslationsAsync()
        {
            return await _context.EntityTranslations.Include(x => x.EntityTranslationFiles).ToListAsync();
        }
    }
}

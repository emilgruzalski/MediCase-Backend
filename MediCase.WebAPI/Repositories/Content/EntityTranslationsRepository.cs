using MediCase.WebAPI.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.WebAPI.Repositories.Content.Interfaces;

namespace MediCase.WebAPI.Repositories.Content
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

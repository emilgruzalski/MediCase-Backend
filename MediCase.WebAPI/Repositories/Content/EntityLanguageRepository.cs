using MediCase.WebAPI.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.WebAPI.Repositories.Content.Interfaces;

namespace MediCase.WebAPI.Repositories.Content
{
    public class EntityLanguageRepository : IEntityLanguageRepository
    {
        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;

        public EntityLanguageRepository(MediCaseContentContext context, ILogger<EntityLanguageRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<EntityLanguage>> GetLanguagesAsync()
        {
            return await _context.EntityLanguages.ToListAsync();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Exceptions;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Entities.Moderator;

namespace MediCase.API.Repositories.Moderator
{
    public class EntityLanguageRepository : IEntityLanguageRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;

        public EntityLanguageRepository(MediCaseModeratorContext context, ILogger<EntityLanguageRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<EntityLanguage>> GetLanguagesAsync()
        {
            return await _context.EntityLanguages.ToListAsync();
        }

        public async Task<uint> AddLanguageAsync(EntityLanguage entityLanguage)
        {
            _context.EntityLanguages.Add(entityLanguage);
            await _context.SaveChangesAsync();
            return entityLanguage.LangId;
        }

        public async Task DeleteLanguageAsync(uint langId)
        {
            var item = _context.EntityLanguages.Where(x => x.LangId == langId).FirstOrDefault();
            if (item == null) throw new NotFoundException($"Language not found: {langId}");
            _context.EntityLanguages.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLanguageAsync(EntityLanguage entityLanguage)
        {
            _context.EntityLanguages.Update(entityLanguage);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetLangValueAsync(uint langId)
        {
            var retrievedLanguage = await _context.EntityLanguages.Where(x => x.LangId == langId).FirstOrDefaultAsync();
            if (retrievedLanguage == null) throw new NotFoundException($"Language not found: {langId}");
            return retrievedLanguage.LangValue;
        }
    }
}

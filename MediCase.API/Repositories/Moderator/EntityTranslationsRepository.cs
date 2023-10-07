using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Exceptions;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Entities.Moderator;

namespace MediCase.API.Repositories.Moderator
{
    public class EntityTranslationsRepository : IEntityTranslationsRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;

        public EntityTranslationsRepository(MediCaseModeratorContext context, ILogger<EntityTranslationsRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<EntityTranslation> AddEntityTranslationAsync(EntityTranslation newEntityTranslation)
        {
            _context.EntityTranslations.Add(newEntityTranslation);
            await _context.SaveChangesAsync();
            return newEntityTranslation;

        }
        public async Task DeleteEntityTranslationAsync(uint entityTranslationId)
        {
            var entityTranslation = _context.EntityTranslations.Where(x => x.TranslationId == entityTranslationId).FirstOrDefault();
            if (entityTranslation == null) throw new NotFoundException($"Translation not found: {entityTranslationId}");
            _context.EntityTranslations.Remove(entityTranslation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntityTranslationAsync(EntityTranslation entityTranslation)
        {
            _context.EntityTranslations.Update(entityTranslation);
            _context.Entry(entityTranslation).Property(x => x.EntityId).IsModified = false;
            await _context.SaveChangesAsync();
        }

        public async Task<List<EntityTranslation>> GetEntityTranslationsAsync()
        {
            return await _context.EntityTranslations.Include(x => x.EntityTranslationFiles).ToListAsync();
        }

        public async Task<EntityTranslation> GetEntityTranslationAsync(uint translationId)
        {
            var translation = await _context.EntityTranslations.Where(x => x.TranslationId == translationId).Include(x => x.Lang).FirstOrDefaultAsync();
            if (translation == null) throw new NotFoundException($"Translation not found: {translationId}");
            return translation;
        }

        public async Task<string> GetEntityTranslationLanguageAsync(uint translationId)
        {
            var retrievedTranslation = await _context.EntityTranslations.Where(x => x.TranslationId == translationId).Include(x => x.Lang).FirstOrDefaultAsync();
            if (retrievedTranslation == null) throw new NotFoundException($"Translation not found: {translationId}");
            return retrievedTranslation.Lang.LangValue;
        }
        public async Task<string?> GetEntityTranslationMainTitleAsync(uint translationId)
        {
            var retrievedTranslation = await _context.EntityTranslations.Where(x => x.TranslationId == translationId).FirstOrDefaultAsync();
            if (retrievedTranslation == null) throw new NotFoundException($"Translation not found: {translationId}");
            return retrievedTranslation.MainTitle;
        }
    }
}

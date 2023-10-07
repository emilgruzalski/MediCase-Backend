using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Exceptions;
using MediCase.API.Entities.Content;
using MediCase.API.Repositories.Content.Interfaces;

namespace MediCase.API.Repositories.Content
{
    public class SynchronizationRepository : ISynchronizationRepository
    {
        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;
        public SynchronizationRepository(ILogger<SynchronizationRepository> logger,
            [FromServices] MediCaseContentContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ImportEntityAsync(Entity entity)
        {
            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

        }
        public async Task DeleteImportedEntityAsync(uint entityId)
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
            _context.ChangeTracker.Clear();
        }
        public async Task UpdateImportedEntitiesAsync(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entities.Update(entity);
                _context.Entry(entity).Property(x => x.TypeId).IsModified = false;
            }
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

        }

        public async Task ImportEntityGraphRelationAsync(EntitiesGraphDatum entityRelation)
        {
            _context.EntitiesGraphData.Add(entityRelation);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        public async Task ImportEntityLanguageAsync(EntityLanguage entityLanguage)
        {
            _context.EntityLanguages.Add(entityLanguage);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

        }
        public async Task DeleteImportedEntityLanguageAsync(uint langId)
        {
            var item = _context.EntityLanguages.Where(x => x.LangId == langId).FirstOrDefault();
            if (item == null) throw new NotFoundException($"Language not found: {langId}");
            _context.EntityLanguages.Remove(item);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
        public async Task UpdateImportedEntityLanguageAsync(EntityLanguage entityLanguage)
        {
            _context.EntityLanguages.Update(entityLanguage);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

        }

        public async Task ImportEntityTranslationAsync(EntityTranslation entityTranslation)
        {
            _context.EntityTranslations.Add(entityTranslation);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
        public async Task DeleteImportedEntityTranslationAsync(uint entityTranslationId)
        {
            var retrievedEntityTranslation = _context.EntityTranslations.Where(x => x.TranslationId == entityTranslationId).FirstOrDefault();
            if (retrievedEntityTranslation == null) throw new NotFoundException($"Translation not found: {entityTranslationId}");
            _context.EntityTranslations.Remove(retrievedEntityTranslation);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
        public async Task UpdateImportedEntityTranslationAsync(EntityTranslation entityTranslation)
        {
            _context.EntityTranslations.Update(entityTranslation);
            _context.Entry(entityTranslation).Property(x => x.EntityId).IsModified = false;
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        public async Task ImportEntityTypeAsync(EntityType entityType)
        {
            _context.EntityTypes.Add(entityType);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
        public async Task DeleteImportedEntityTypeAsync(uint entityTypeId)
        {
            var retrievedEntityType = _context.EntityTypes.Where(x => x.TypeId == entityTypeId).FirstOrDefault();
            if (retrievedEntityType == null) throw new NotFoundException($"Type not found: {entityTypeId}");
            _context.EntityTypes.Remove(retrievedEntityType);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
        public async Task UpdateImportedEntityTypeAsync(EntityType entityType)
        {
            _context.EntityTypes.Update(entityType);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        public async Task ImportEntityTranslationFileAsync(EntityTranslationFile translationFile)
        {
            _context.EntityTranslationFiles.Add(translationFile);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
        public async Task DeleteImportedEntityTranslationFileAsync(uint translationFileId)
        {
            var retrievedEntityTranslationFile = _context.EntityTranslationFiles.Where(x => x.FileId == translationFileId).FirstOrDefault();
            if (retrievedEntityTranslationFile == null) throw new NotFoundException($"File not found: {translationFileId}");
            _context.EntityTranslationFiles.Remove(retrievedEntityTranslationFile);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
        public async Task UpdateImportedEntityTranslationFileAsync(EntityTranslationFile translationFile)
        {
            _context.EntityTranslationFiles.Update(translationFile);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
    }
}

using MediCase.API.Entities.Content;

namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface ISynchronizationRepository
    {

        Task ImportEntityAsync(Entity entity);
        Task DeleteImportedEntityAsync(uint entityId);
        Task UpdateImportedEntitiesAsync(List<Entity> entities);

        Task ImportEntityGraphRelationAsync(EntitiesGraphDatum entityRelation);

        Task ImportEntityLanguageAsync(EntityLanguage entityLanguage);
        Task DeleteImportedEntityLanguageAsync(uint langId);
        Task UpdateImportedEntityLanguageAsync(EntityLanguage entityLanguage);

        Task ImportEntityTranslationAsync(EntityTranslation entityTranslation);
        Task DeleteImportedEntityTranslationAsync(uint entityTranslationId);
        Task UpdateImportedEntityTranslationAsync(EntityTranslation entityTranslation);

        Task ImportEntityTypeAsync(EntityType entityType);
        Task DeleteImportedEntityTypeAsync(uint entityTypeId);
        Task UpdateImportedEntityTypeAsync(EntityType entityType);

        Task ImportEntityTranslationFileAsync(EntityTranslationFile translationFile);
        Task DeleteImportedEntityTranslationFileAsync(uint translationFileId);
        Task UpdateImportedEntityTranslationFileAsync(EntityTranslationFile translationFile);




    }
}

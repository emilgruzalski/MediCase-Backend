using MediCase.WebAPI.Models.Entity;

namespace MediCase.WebAPI.Services.Interfaces
{
    public interface IModEntityService
    {
        Task<List<EntityTypeDto>> GetEntityTypesAsync();
        Task UpdateEntityTypeAsync(EntityTypeDto entityType);
        Task<uint> AddEntityTypeAsync(string entityType);
        Task DeleteEntityTypeAsync(uint typeId);

        Task<List<EntityLanguageGetDto>> GetLanguagesAsync();
        Task<uint> AddLanguageAsync(string langCode);
        Task DeleteLanguageAsync(uint langId);
        Task UpdateLanguageAsync(EntityLanguageUpdateDto entityLanguage);

        Task<uint> AddEntityAsync(uint? parentId, EntityPostDto newEntity);
        Task DeleteEntityAsync(uint entityId);
        Task UpdateEntityOrderAsync(List<EntityUpdateDto> entity);
        Task<List<EntityDto>> GetEntitiesAsync();
        Task<EntityWithTranslationDto> GetEntityWithTranslationsAsync(uint entityId);
        Task<List<EntityWithTranslationDto>> GetEntitiesWithTranslationsAsync();

        Task<EntityGraphObjectDto> GetEntityWithChildsAsync(uint? entityId);

        Task<EntityGraphObjectDto> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs);

        Task<List<EntityWithTranslationDto>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs);

        Task<uint> AddEntityTranslationAsync(EntityTranslationPostDto newEntityTranslation);
        Task DeleteEntityTranslationAsync(uint entityTranslationId);
        Task UpdateEntityTranslationAsync(EntityTranslationUpdateDto entityTranslation);
        Task<List<EntityTranslationDto>> GetEntityTranslationsAsync();


        Task<uint> AddEntityTranslationFileAsync(EntityTranslationFilePostDto newEntityTranslationFile);
        Task DeleteEntityTranslationFileAsync(uint entityTranslationFileId);
        Task<Stream> GetEntityTranslationFileAsync(string filePath);

        Task<bool> CheckIfWeHaveChangesAsync();

        Task RefreshEntityLockAsync(uint entityId, uint newTime);

        Task<bool> IsEntityLockedAsync(uint entityId);
    }
}

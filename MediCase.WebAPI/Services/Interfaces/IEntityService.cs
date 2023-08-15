using MediCase.WebAPI.Models.Entity;

namespace MediCase.WebAPI.Services.Interfaces
{
    public interface IEntityService
    {
        Task<List<EntityTypeDto>> GetEntityTypesAsync();

        Task<List<EntityLanguageGetDto>> GetLanguagesAsync();

        Task<List<EntityDto>> GetEntitiesAsync();
        Task<EntityWithTranslationDto> GetEntityWithTranslationsAsync(uint entityId);
        Task<List<EntityWithTranslationDto>> GetEntitiesWithTranslationsAsync();

        Task<EntityGraphObjectDto> GetEntityWithChildsAsync(uint? entityId);
        Task<EntityGraphObjectDto> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs);

        Task<List<EntityWithTranslationDto>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs);

        Task<List<EntityTranslationDto>> GetEntityTranslationsAsync();

        Task<Stream> GetEntityTranslationFileAsync(string filePath);

    }
}

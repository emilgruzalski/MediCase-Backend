using MediCase.API.Entities.Moderator;

namespace MediCase.API.Repositories.Moderator.Interfaces
{
    public interface IEntityTranslationsRepository
    {
        Task<EntityTranslation> AddEntityTranslationAsync(EntityTranslation newEntityTranslation);
        Task DeleteEntityTranslationAsync(uint entityTranslationId);

        Task UpdateEntityTranslationAsync(EntityTranslation entityTranslation);

        Task<List<EntityTranslation>> GetEntityTranslationsAsync();

        Task<EntityTranslation> GetEntityTranslationAsync(uint translationId);

        Task<string> GetEntityTranslationMainTitleAsync(uint translationId);

        Task<string> GetEntityTranslationLanguageAsync(uint translationId);


    }
}

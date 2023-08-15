using MediCase.WebAPI.Entities.Moderator;

namespace MediCase.WebAPI.Repositories.Moderator.Interfaces
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

using MediCase.API.Entities.Moderator;

namespace MediCase.API.Repositories.Moderator.Interfaces
{
    public interface IEntityLanguageRepository
    {
        Task<string> GetLangValueAsync(uint langId);
        Task<List<EntityLanguage>> GetLanguagesAsync();

        Task<uint> AddLanguageAsync(EntityLanguage entityLanguage);

        Task DeleteLanguageAsync(uint langId);

        Task UpdateLanguageAsync(EntityLanguage entityLanguage);
    }
}

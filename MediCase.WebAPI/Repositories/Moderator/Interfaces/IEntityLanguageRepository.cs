using MediCase.WebAPI.Entities.Moderator;

namespace MediCase.WebAPI.Repositories.Moderator.Interfaces
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

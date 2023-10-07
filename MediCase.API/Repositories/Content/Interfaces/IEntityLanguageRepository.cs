using MediCase.API.Entities.Content;

namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface IEntityLanguageRepository
    {
        Task<List<EntityLanguage>> GetLanguagesAsync();
    }
}

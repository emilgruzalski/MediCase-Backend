using MediCase.WebAPI.Entities.Content;

namespace MediCase.WebAPI.Repositories.Content.Interfaces
{
    public interface IEntityLanguageRepository
    {
        Task<List<EntityLanguage>> GetLanguagesAsync();
    }
}

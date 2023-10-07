using MediCase.API.Entities.Content;

namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface IEntityTranslationsRepository
    {
        Task<List<EntityTranslation>> GetEntityTranslationsAsync();
    }
}

using MediCase.WebAPI.Entities.Content;

namespace MediCase.WebAPI.Repositories.Content.Interfaces
{
    public interface IEntityTranslationsRepository
    {
        Task<List<EntityTranslation>> GetEntityTranslationsAsync();
    }
}

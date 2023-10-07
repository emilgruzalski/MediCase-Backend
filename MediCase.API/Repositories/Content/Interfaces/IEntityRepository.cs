using MediCase.API.Entities.Content;

namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface IEntityRepository
    {
        Task<List<Entity>> GetEntitiesAsync();
        Task<Entity> GetEntityWithTranslationsAsync(uint entityId);
        Task<List<Entity>> GetEntitiesWithTranslationsAsync();

    }
}

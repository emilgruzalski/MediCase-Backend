using MediCase.WebAPI.Entities.Content;

namespace MediCase.WebAPI.Repositories.Content.Interfaces
{
    public interface IEntityRepository
    {
        Task<List<Entity>> GetEntitiesAsync();
        Task<Entity> GetEntityWithTranslationsAsync(uint entityId);
        Task<List<Entity>> GetEntitiesWithTranslationsAsync();

    }
}

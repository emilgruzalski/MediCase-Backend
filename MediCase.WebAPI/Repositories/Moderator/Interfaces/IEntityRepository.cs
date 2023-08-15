using MediCase.WebAPI.Entities.Moderator;

namespace MediCase.WebAPI.Repositories.Moderator.Interfaces
{
    public interface IEntityRepository
    {
        Task RefreshEntityLockAsync(uint entityId, uint newTime);
        Task<Entity> AddEntityAsync(Entity entity);
        Task DeleteEntityAsync(uint entityId);
        Task UpdateEntityOrderAsync(List<Entity> entity);
        Task<List<Entity>> GetEntitiesAsync();

        Task<Entity> GetEntityWithTranslationsAsync(uint entityId);
        Task<List<Entity>> GetEntitiesWithTranslationsAsync();

        Task<Entity> MarkEntityHasChilds(uint? entityId);
        Task<bool> IsEntityLockedAsync(uint entityId);

    }
}

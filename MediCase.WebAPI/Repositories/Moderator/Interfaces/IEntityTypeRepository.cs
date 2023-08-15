using MediCase.WebAPI.Entities.Moderator;

namespace MediCase.WebAPI.Repositories.Moderator.Interfaces
{
    public interface IEntityTypeRepository
    {
        Task<List<EntityType>> GetEntityTypesAsync();

        Task UpdateEntityTypeAsync(EntityType entityType);

        Task<EntityType> AddEntityTypeAsync(EntityType newEntityType);

        Task DeleteEntityTypeAsync(uint typeId);


    }
}

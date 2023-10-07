using MediCase.API.Entities.Moderator;

namespace MediCase.API.Repositories.Moderator.Interfaces
{
    public interface IEntityTypeRepository
    {
        Task<List<EntityType>> GetEntityTypesAsync();

        Task UpdateEntityTypeAsync(EntityType entityType);

        Task<EntityType> AddEntityTypeAsync(EntityType newEntityType);

        Task DeleteEntityTypeAsync(uint typeId);


    }
}

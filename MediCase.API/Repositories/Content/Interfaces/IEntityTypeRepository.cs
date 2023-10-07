using MediCase.API.Entities.Content;

namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface IEntityTypeRepository
    {
        Task<List<EntityType>> GetEntityTypesAsync();

    }
}

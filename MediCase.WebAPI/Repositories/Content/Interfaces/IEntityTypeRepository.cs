using MediCase.WebAPI.Entities.Content;

namespace MediCase.WebAPI.Repositories.Content.Interfaces
{
    public interface IEntityTypeRepository
    {
        Task<List<EntityType>> GetEntityTypesAsync();

    }
}

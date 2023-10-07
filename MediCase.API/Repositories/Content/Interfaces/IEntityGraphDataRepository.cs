using MediCase.API.Entities.Content;

namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface IEntityGraphDataRepository
    {
        Task<Entity> GetEntityWithChildsAsync(uint? entityId);
        Task<Entity> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs);
        Task<List<EntitiesGraphDatum>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs);
    }
}

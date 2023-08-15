using MediCase.WebAPI.Entities.Content;

namespace MediCase.WebAPI.Repositories.Content.Interfaces
{
    public interface IEntityGraphDataRepository
    {
        Task<Entity> GetEntityWithChildsAsync(uint? entityId);
        Task<Entity> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs);
        Task<List<EntitiesGraphDatum>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs);
    }
}

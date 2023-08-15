using MediCase.WebAPI.Entities.Moderator;

namespace MediCase.WebAPI.Repositories.Moderator.Interfaces
{
    public interface IEntityGraphDataRepository
    {
        Task<Entity> GetEntityWithChildsAsync(uint? entityId);
        Task<Entity> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs);
        Task<List<EntitiesGraphDatum>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs);
        Task<EntitiesGraphDatum> AttachEntityToParentAsync(EntitiesGraphDatum entityRelation);
    }
}

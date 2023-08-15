using MediCase.WebAPI.Entities.Moderator;

namespace MediCase.WebAPI.Repositories.Moderator.Interfaces
{
    public interface IModeratorQueryBucketRepository
    {
        Task<List<ModeratorQueryBucket>> GetPendingChangesAsync();

        Task AddBucketItemAsync(ModeratorQueryBucket bucketItem);

        Task TrucateTableAsync();

        Task<bool> CheckIfWeHaveChangesAsync();

        Task DeleteChangeFromQueueAsync(ulong operationId);
    }
}

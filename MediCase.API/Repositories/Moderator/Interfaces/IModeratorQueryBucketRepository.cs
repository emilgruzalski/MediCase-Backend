using MediCase.API.Entities.Moderator;

namespace MediCase.API.Repositories.Moderator.Interfaces
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.WebAPI.Entities.Moderator;
using MediCase.WebAPI.Repositories.Moderator.Interfaces;
using MediCase.WebAPI.Exceptions;
namespace MediCase.WebAPI.Repositories.Moderator
{
    public class ModeratorQueryBucketRepository : IModeratorQueryBucketRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;

        public ModeratorQueryBucketRepository(MediCaseModeratorContext context, ILogger<ModeratorQueryBucketRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<ModeratorQueryBucket>> GetPendingChangesAsync()
        {
            return await _context.ModeratorQueryBuckets.OrderBy(x => x.OperationId).ToListAsync();
        }

        public async Task AddBucketItemAsync(ModeratorQueryBucket bucketItem)
        {
            _context.ModeratorQueryBuckets.Add(bucketItem);
            await _context.SaveChangesAsync();
        }

        public async Task TrucateTableAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("truncate table ModeratorQueryBucket;");
        }

        public async Task DeleteChangeFromQueueAsync(ulong operationId)
        {
            var returnedChange = await _context.ModeratorQueryBuckets.Where(x => x.OperationId == operationId).FirstOrDefaultAsync();
            if (returnedChange == null) throw new NotFoundException($"Item in queue missing: {operationId}");
            _context.ModeratorQueryBuckets.Remove(returnedChange);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckIfWeHaveChangesAsync()
        {
            return await _context.ModeratorQueryBuckets.AnyAsync();
        }
    }
}

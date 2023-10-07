using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Entities.Moderator;

namespace MediCase.API.Repositories.Moderator
{
    public class MediCaseModTransactionRepository : IMediCaseModTransactionRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;
        public MediCaseModTransactionRepository(MediCaseModeratorContext context,
            ILogger<MediCaseModTransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }
        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using MediCase.WebAPI.Entities.Moderator;
using MediCase.WebAPI.Repositories.Moderator.Interfaces;

namespace MediCase.WebAPI.Repositories.Moderator
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

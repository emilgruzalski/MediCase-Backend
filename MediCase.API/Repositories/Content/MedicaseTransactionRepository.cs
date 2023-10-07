using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MediCase.API.Entities.Content;
using MediCase.API.Repositories.Content.Interfaces;

namespace MediCase.API.Repositories.Content
{
    public class MediCaseTransactionRepository : IMediCaseTransactionRepository
    {

        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;
        public MediCaseTransactionRepository(MediCaseContentContext context,
            ILogger<MediCaseTransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted);
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

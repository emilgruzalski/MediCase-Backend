using Microsoft.EntityFrameworkCore.Storage;

namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface IMediCaseTransactionRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
    }
}

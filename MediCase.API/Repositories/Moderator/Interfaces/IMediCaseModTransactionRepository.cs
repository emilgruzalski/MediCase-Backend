﻿using Microsoft.EntityFrameworkCore.Storage;

namespace MediCase.API.Repositories.Moderator.Interfaces
{
    public interface IMediCaseModTransactionRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
    }
}

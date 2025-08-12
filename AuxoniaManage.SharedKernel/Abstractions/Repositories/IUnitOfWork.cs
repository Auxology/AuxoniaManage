using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuxoniaManage.SharedKernel.Abstractions.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    
    Task CommitAsync(CancellationToken cancellationToken = default);
    
    Task RollbackAsync(CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
};
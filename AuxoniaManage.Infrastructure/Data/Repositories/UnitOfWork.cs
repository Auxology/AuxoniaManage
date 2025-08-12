using System.Data;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuxoniaManage.Infrastructure.Data.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _ownsTransaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;
    }

    public async Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            return _transaction;
        }
        
        if (_context.Database.CurrentTransaction != null)
        {
            _transaction = _context.Database.CurrentTransaction;
            _ownsTransaction = false; // We don't own this transaction
            return _transaction;
        }
        
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        _ownsTransaction = true; // We created this transaction
        
        return _transaction;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            // Only dispose if we own the transaction
            if (_ownsTransaction)
            {
                _transaction.Dispose();
            }
            _transaction = null;
            _ownsTransaction = false;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            // Only dispose if we own the transaction
            if (_ownsTransaction)
            {
                _transaction.Dispose();
            }
            _transaction = null;
            _ownsTransaction = false;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
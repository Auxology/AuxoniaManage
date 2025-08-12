using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Pipelines;

public sealed class TransactionalPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalCommand<TResponse>
{
    private readonly ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionalPipelineBehavior
    (
        ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting transaction for request: {RequestType}", typeof(TRequest).Name);
        
        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            TResponse response = await next(cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
            
            _logger.LogInformation("Transaction committed for request: {RequestType}", typeof(TRequest).Name);

            return response;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
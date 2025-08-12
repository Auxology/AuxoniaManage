using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Pipelines; 

public sealed class ExceptionHandlingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingPipelineBehavior
    (
        ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> logger
    )
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request: {Request}", request);
            
            throw;
        }
    }
}
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Pipelines;

public sealed class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    
    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestSource = typeof(TRequest).Namespace;
        var requestTime = DateTime.UtcNow;
        
        _logger.LogInformation(
            "Handling request {RequestName} from source {RequestSource} at {RequestTime}",
            requestName,
            requestSource,
            requestTime
        );
        
        var response = await next(cancellationToken);
        
        _logger.LogInformation(
            "Handled request {RequestName} from source {RequestSource} at {RequestTime}",
            requestName,
            requestSource,
            DateTime.UtcNow
        );
        
        return response;
    }
}
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Storage;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Storage;

public sealed class ObjectRemovedStorageConsumer : IConsumer<ObjectRemovedEvent>
{
    private readonly IStorageService _storageService;
    private readonly ILogger<ObjectRemovedStorageConsumer> _logger;

    public ObjectRemovedStorageConsumer
    (
        IStorageService storageService, ILogger<ObjectRemovedStorageConsumer> logger
    )

    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ObjectRemovedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Object removing event being processed: {Key}", message.Key);
        
        var result = await _storageService.DeleteObjectAsync(message.Key, context.CancellationToken);
        
        if (result)
        {
            _logger.LogInformation("Object removed successfully: {Key}", message.Key);
        }
        else
        {
            _logger.LogWarning("Failed to remove object: {Key}", message.Key);
        }
    }
}
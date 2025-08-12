using AuxoniaManage.Application.Features.Profile.CreateReadModel;
using AuxoniaManage.Domain.Events.Profile;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Profile;

public sealed class ProfileCreatedConsumer : IConsumer<ProfileCreatedEvent>
{
    private readonly ILogger<ProfileCreatedConsumer> _logger;
    private readonly IMediator _mediator;

    public ProfileCreatedConsumer
    (
        ILogger<ProfileCreatedConsumer> logger,
        IMediator mediator
    )

    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProfileCreatedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("ProfileCreatedConsumer: Consuming message with UserId: {UserId}", message.UserId);
        
        var command = new CreateProfileReadModelCommand
        (
            ProfileId: message.Id,
            UserId: message.UserId,
            FullName: message.FullName,
            Email: message.Email
        );
        
        var response = await _mediator.Send(command, context.CancellationToken);
        
        _logger.LogInformation("ProfileCreatedConsumer: Successfully created read model for UserId: {UserId}, ReadModelId: {ReadModelId}", 
            message.UserId, response.Id);
    }
}
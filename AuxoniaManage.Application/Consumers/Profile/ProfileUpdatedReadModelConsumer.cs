using AuxoniaManage.Application.Features.Profile.UpdateReadModel;
using AuxoniaManage.Domain.Events.Profile;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Profile;

public sealed class ProfileUpdatedReadModelConsumer : IConsumer<ProfileUpdatedEvent>
{
    private readonly ILogger<ProfileUpdatedReadModelConsumer> _logger;
    private readonly IMediator _mediator;
    
    public ProfileUpdatedReadModelConsumer
    (
        ILogger<ProfileUpdatedReadModelConsumer> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProfileUpdatedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Updating read model for profile {ProfileId} of user {UserId}", message.Id, message.UserId);

        var command = new UpdateProfileReadModelCommand
        (
            ProfileId: message.Id,
            UserId: message.UserId,
            FullName: message.FullName,
            AvatarKey: message.AvatarKey
        );

        var response = await _mediator.Send(command, context.CancellationToken);

        _logger.LogInformation("Successfully updated read model for profile {ProfileId} of user {UserId}, ReadModelId: {ReadModelId}", 
            message.Id, message.UserId, response.Id);
    }
}
using AuxoniaManage.Application.Features.Workspace.UpdateReadModel;
using AuxoniaManage.Domain.Events.Workspace;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Workspace;

public sealed class WorkspaceUpdatedReadModelConsumer : IConsumer<WorkspaceUpdatedEvent>
{
    private readonly ILogger<WorkspaceUpdatedReadModelConsumer> _logger;
    private readonly IMediator _mediator;
    
    public WorkspaceUpdatedReadModelConsumer
    (
        ILogger<WorkspaceUpdatedReadModelConsumer> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<WorkspaceUpdatedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Updating read model for workspace {WorkspaceId}", message.WorkspaceId);

        var command = new UpdateWorkspaceReadModelCommand
        (
            WorkspaceId: message.WorkspaceId,
            Name: message.Name,
            LogoKey: message.LogoKey
        );

        var response = await _mediator.Send(command, context.CancellationToken);

        _logger.LogInformation("Successfully updated read model for workspace {WorkspaceId}, ReadModelId: {ReadModelId}", 
            message.WorkspaceId, response.Id);
    }
}
using AuxoniaManage.Application.Features.Workspace.DeleteReadModel;
using AuxoniaManage.Domain.Events.Workspace;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Workspace;

public sealed class WorkspaceDeletedReadModelConsumer : IConsumer<WorkspaceDeletedEvent>
{
    private readonly ILogger<WorkspaceDeletedReadModelConsumer> _logger;
    private readonly IMediator _mediator;

    public WorkspaceDeletedReadModelConsumer
    (
        ILogger<WorkspaceDeletedReadModelConsumer> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<WorkspaceDeletedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("WorkspaceDeletedReadModelConsumer: Consuming message with WorkspaceId: {WorkspaceId}", message.WorkspaceId);
        
        var command = new DeleteWorkspaceReadModelCommand
        (
            WorkspaceId: message.WorkspaceId
        );
        
        var response = await _mediator.Send(command, context.CancellationToken);
        
        if (response.IsDeleted)
        {
            _logger.LogInformation("WorkspaceDeletedReadModelConsumer: Successfully deleted read model for WorkspaceId: {WorkspaceId}", message.WorkspaceId);
        }
        else
        {
            _logger.LogWarning("WorkspaceDeletedReadModelConsumer: Failed to delete read model or workspace not found for WorkspaceId: {WorkspaceId}", message.WorkspaceId);
        }
    }
}
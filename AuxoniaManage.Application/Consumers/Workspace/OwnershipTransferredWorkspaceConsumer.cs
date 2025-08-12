using AuxoniaManage.Application.Features.Workspace.UpdateOwner;
using AuxoniaManage.Domain.Events.Membership;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Workspace;

public sealed class OwnershipTransferredWorkspaceConsumer : IConsumer<OwnershipTransferredEvent>
{
    private readonly ILogger<OwnershipTransferredWorkspaceConsumer> _logger;
    private readonly IMediator _mediator;

    public OwnershipTransferredWorkspaceConsumer
    (
        ILogger<OwnershipTransferredWorkspaceConsumer> logger,
        IMediator mediator
    )

    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<OwnershipTransferredEvent> context)
    {
        var message = context.Message;
        
        var command = new UpdateWorkspaceOwnerCommand
        (
            message.NewOwnerId,
            message.WorkspaceId
        );
        
        _logger.LogInformation("Received OwnershipTransferredEvent for WorkspaceId: {WorkspaceId}, NewOwnerId: {NewOwnerId}", 
            message.WorkspaceId, message.NewOwnerId);
        
        var response = await _mediator.Send(command);
        
        _logger.LogInformation("Ownership transferred for WorkspaceId: {WorkspaceId}, NewOwnerId: {NewOwnerId}, Result: {Result}", 
            message.WorkspaceId, message.NewOwnerId, response);
    }
}
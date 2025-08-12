using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Workspace;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;

namespace AuxoniaManage.Application.Features.Workspace.Delete;

public sealed class DeleteWorkspaceCommandHandler : ICommandHandler<DeleteWorkspaceCommand, DeleteWorkspaceResponse>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteWorkspaceCommandHandler
    (
        IWorkspaceRepository workspaceRepository,
        IPublishEndpoint publishEndpoint
    )
    {
        _workspaceRepository = workspaceRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<DeleteWorkspaceResponse> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var workspace = await _workspaceRepository.GetAsync(request.WorkspaceId, cancellationToken);
        
        if (workspace is null)
        {
            throw new WorkspaceNotFoundException();
        }
        
        if (workspace.OwnerId != request.UserId)
        {
            throw new OnlyOwnerCanDeleteWorkspaceException();
        }
        
        var isSuccess = await _workspaceRepository.DeleteAsync(workspace, cancellationToken);
        
        if (!isSuccess)
        {
            throw new WorkspaceDeletionFailedException();
        }
        
        var workspaceDeletedEvent = new WorkspaceDeletedEvent
        (
            WorkspaceId: request.WorkspaceId,
            Name: workspace.Name
        );
        
        await _publishEndpoint.Publish(workspaceDeletedEvent, cancellationToken);
        
        return new DeleteWorkspaceResponse
        (
            request.WorkspaceId,
            request.UserId,
            DateTime.UtcNow
        );
    }
}
using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Storage;
using AuxoniaManage.Domain.Events.Workspace;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using MediatR;

namespace AuxoniaManage.Application.Features.Workspace.Update;

public sealed class UpdateWorkspaceCommandHandler : ICommandHandler<UpdateWorkspaceCommand, UpdateWorkspaceResponse>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IStorageService _storageService;
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateWorkspaceCommandHandler
    (
        IWorkspaceRepository workspaceRepository,
        IStorageService storageService,
        IMediator mediator,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _workspaceRepository = workspaceRepository;
        _storageService = storageService;
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<UpdateWorkspaceResponse> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrWhiteSpace(request.UserId, nameof(request.UserId));
        
        var workspace = await _workspaceRepository.GetAsync(request.WorkspaceId, cancellationToken);
        
        if (workspace == null)
        {
            throw new WorkspaceNotFoundException(request.WorkspaceId);
        }
        
        if (workspace.OwnerId != request.UserId)
        {
            throw new OnlyWorkspaceOwnerCanUpdateException();
        }
        
        var name = request.Name ?? workspace.Name;
        var description = request.Description ?? workspace.Description;
        var oldKey = workspace.LogoKey;
        var newKey = oldKey;
        
        if (request.Logo != null)
        {
            if (!string.IsNullOrWhiteSpace(oldKey))
            {
                var objectRemovedEvent = new ObjectRemovedEvent(oldKey);
                await _publishEndpoint.Publish(objectRemovedEvent, cancellationToken);
            }

            newKey = await _storageService.PutObjectAsync
            (
                file: request.Logo.OpenReadStream(),
                sender: Senders.Workspace,
                request.Logo.FileName,
                request.Logo.ContentType,
                request.Logo.Length,
                cancellationToken
            );
        }
        
        workspace.UpdateWorkspace
        (
            name: name,
            description: description,
            timeStamp: DateTime.UtcNow,
            logoKey: newKey
        );
        
        var isUpdated = await _workspaceRepository.UpdateAsync(workspace, cancellationToken);
        
        if (!isUpdated)
        {
            throw new WorkspaceUpdateFailedException();
        }
        
        var workspaceUpdatedEvent = new WorkspaceUpdatedEvent
        (
            Id: workspace.Id,
            WorkspaceId: workspace.Id,
            Name: workspace.Name,
            LogoKey: workspace.LogoKey
        );
        
        await _publishEndpoint.Publish(workspaceUpdatedEvent, cancellationToken);
        
        return new UpdateWorkspaceResponse
        (
            Id: workspace.Id,
            Name: workspace.Name,
            Description: workspace.Description,
            UpdatedAt: workspace.UpdatedAt,
            LogoKey: workspace.LogoKey
        );
    }
}
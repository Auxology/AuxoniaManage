using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Project;
using AuxoniaManage.Domain.Events.Storage;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;

namespace AuxoniaManage.Application.Features.Projects.Delete;

public sealed class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand, DeleteProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteProjectCommandHandler
    (
        IProjectRepository projectRepository,
        IWorkspacePermissionService workspacePermissionService,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _projectRepository = projectRepository;
        _workspacePermissionService = workspacePermissionService;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<DeleteProjectResponse> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));
        
        var hasPermission = await _workspacePermissionService.IsOwnerAsync(
            request.WorkspaceId, request.UserId, cancellationToken);

        if (!hasPermission)
        {
            throw new UserDoesNotHavePermissionException();
        }
        
        var project = await _projectRepository.GetAsync(request.ProjectId, cancellationToken);
        
        if (project == null || project.WorkspaceId != request.WorkspaceId)
        {
            throw new ProjectNotFoundException();
        }

        if (!string.IsNullOrEmpty(project.LogoKey))
        {
            var objectRemovedEvent = new ObjectRemovedEvent(project.LogoKey);
            
            await _publishEndpoint.Publish(objectRemovedEvent, cancellationToken);
        }
        
        var isSuccess = await _projectRepository.DeleteAsync(project, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProjectDeletionFailedException();
        }
        
        var projectDeletedEvent = new ProjectDeletedEvent
        (
            project.Id,
            project.WorkspaceId
        );
        
        await _publishEndpoint.Publish(projectDeletedEvent, cancellationToken);
        
        return new DeleteProjectResponse
        (
            DeletedById: request.UserId,
            WorkspaceId: request.WorkspaceId,
            ProjectId: request.ProjectId,
            DeletedAt: DateTime.UtcNow
        );
    }
}
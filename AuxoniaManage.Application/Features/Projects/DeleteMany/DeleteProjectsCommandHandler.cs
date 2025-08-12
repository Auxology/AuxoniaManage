using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Features.Workspace;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Project;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;

namespace AuxoniaManage.Application.Features.Projects.DeleteMany;

public sealed class DeleteProjectsCommandHandler : ICommandHandler<DeleteProjectsCommand, DeleteProjectsResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteProjectsCommandHandler
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
    
    public async Task<DeleteProjectsResponse> Handle(DeleteProjectsCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var isOwner = await _workspacePermissionService.IsOwnerAsync(request.WorkspaceId, request.UserId, cancellationToken);

        if (!isOwner)
        {
            throw new UserDoesNotHavePermissionException();
        }
        
        var projects = await _projectRepository.GetAllAsync(request.WorkspaceId, cancellationToken);

        if (projects.Count == 0)
        {
            return new DeleteProjectsResponse
            (
                request.UserId,
                request.WorkspaceId,
                [],
                DateTime.UtcNow
            );
        }
        
        var projectIds = projects.Select(p => p.Id).ToList();

        var isSuccess = await _projectRepository.DeleteRangeAsync(projects, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProjectDeletionFailedException();
        }
        
        var projectsDeletedEvent = new ProjectsDeletedEvent
        (
            projectIds,
            request.WorkspaceId
        );
        
        await _publishEndpoint.Publish(projectsDeletedEvent, cancellationToken);
        
        return new DeleteProjectsResponse
        (
            request.UserId,
            request.WorkspaceId,
            projectIds,
            DateTime.UtcNow
        );
    }
}
using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.ProjectTask.DeleteMany;

public sealed class DeleteProjectTasksCommandHandler : ICommandHandler<DeleteProjectTasksCommand, DeleteProjectTasksResponse>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IReadModelRepository _readModelRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    
    public DeleteProjectTasksCommandHandler
    (
        IProjectTaskRepository projectTaskRepository,
        IReadModelRepository readModelRepository,
        IWorkspacePermissionService workspacePermissionService
    )
    {
        _projectTaskRepository = projectTaskRepository;
        _readModelRepository = readModelRepository;
        _workspacePermissionService = workspacePermissionService;
    }
    
    
    public async Task<DeleteProjectTasksResponse> Handle(DeleteProjectTasksCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.ProjectIds, nameof(request.ProjectIds));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var userRole =
            await _workspacePermissionService.GetRoleAsync(request.WorkspaceId, request.UserId, cancellationToken);

        if (userRole == WorkspaceRoles.Member)
        {
            throw new LackOfPermissionException();
        }
        
        var tasks = await _projectTaskRepository.GetByProjectIdsAsync(request.ProjectIds, cancellationToken);
        
        if (tasks.Count == 0)
        {
            return new DeleteProjectTasksResponse
            (
                DeletedTaskIds: [],
                WorkspaceId: request.WorkspaceId,
                ProjectIds: request.ProjectIds,
                DeletedById: request.UserId,
                DateTime.UtcNow
            );
        }
        
        var isSuccess = await _projectTaskRepository.DeleteRangeAsync(tasks, cancellationToken);
        
        if (!isSuccess)
        {
            throw new CouldNotDeleteTaskException();
        }
        
        return new DeleteProjectTasksResponse
        (
            DeletedTaskIds: tasks.Select(t => t.Id).ToList(),
            WorkspaceId: request.WorkspaceId,
            ProjectIds: request.ProjectIds,
            DeletedById: request.UserId,
            DateTime.UtcNow
        );
    }
}
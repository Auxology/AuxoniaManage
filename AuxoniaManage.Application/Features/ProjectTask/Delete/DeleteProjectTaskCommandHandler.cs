using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.ProjectTask.Delete;

public sealed class DeleteProjectTaskCommandHandler : ICommandHandler<DeleteProjectTaskCommand, DeleteProjectTaskResponse>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IReadModelRepository _readModelRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;

    public DeleteProjectTaskCommandHandler
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
    
    public async Task<DeleteProjectTaskResponse> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.Id, nameof(request.Id));
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var projectReadModel = await _readModelRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        if (projectReadModel == null)
        {
            throw new CouldNotFindProjectException();
        }

        var userRole =
            await _workspacePermissionService.GetRoleAsync(request.WorkspaceId, request.UserId, cancellationToken);

        if (userRole == WorkspaceRoles.Member)
        {
            throw new LackOfPermissionException();
        }
        
        var task = await _projectTaskRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task == null)
        {
            throw new TaskNotFoundException();
        }
        
        var canDeleteTask = CanDeleteTask(request.UserId, userRole, task.AssignedById);
        
        if (!canDeleteTask)
        {
            throw new LackOfPermissionException();
        }
        
        var isSuccess = await _projectTaskRepository.DeleteAsync(task, cancellationToken);
        
        if (!isSuccess)
        {
            throw new CouldNotDeleteTaskException();
        }
        
        return new DeleteProjectTaskResponse
        (
            Id: task.Id,
            WorkspaceId: request.WorkspaceId,
            ProjectId: request.ProjectId,
            DeletedById: request.UserId,
            DeletedAt: DateTime.UtcNow
        );
    }
    
    private bool CanDeleteTask(string userId, WorkspaceRoles userRole, string assignedById)
    {
        return userRole switch
        {
            WorkspaceRoles.Owner => true,
            WorkspaceRoles.Admin => assignedById == userId,
            _ => false
        };
    }
}
using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.ProjectTask;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.ProjectTask.Edit;

public sealed class EditTaskCommandHandler : ICommandHandler<EditTaskCommand, EditTaskCommandResponse>
{
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IReadModelRepository _readModelRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public EditTaskCommandHandler
    (
        IWorkspacePermissionService workspacePermissionService,
        IProjectTaskRepository projectTaskRepository,
        IReadModelRepository readModelRepository,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _workspacePermissionService = workspacePermissionService;
        _projectTaskRepository = projectTaskRepository;
        _readModelRepository = readModelRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<EditTaskCommandResponse> Handle(EditTaskCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.Id, nameof(request.Id));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));
        Guard.Against.NullOrEmpty(request.AssigneeIds, nameof(request.AssigneeIds));
        
        if (request.AssigneeIds.Contains(request.UserId))
        {
            throw new CannotAssignTaskToSelfException();
        }
        
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

        var canEditTask = CanEditTask(request.UserId, userRole, task.AssignedById);

        if (!canEditTask)
        {
            throw new LackOfPermissionException();
        }

        task.UpdateProjectTask
        (
            assigneeIds: request.AssigneeIds.ToList(),
            title: request.Title ?? task.Title,
            description: request.Description ?? task.Description,
            deadlineAt: request.DeadlineAt ?? task.DeadlineAt,
            status: request.Status ?? task.Status,
            priority: request.Priority ?? task.Priority,
            timeStamp: DateTime.UtcNow
        );

        var isSuccess = await _projectTaskRepository.UpdateAsync(task, cancellationToken);
        
        if (!isSuccess)
        {
            throw new FailedToEditTaskException();
        }
        
        var taskEditedEvent = new ProjectTaskEditedEvent
        (
            Id: task.Id,
            WorkspaceId: request.WorkspaceId,
            ProjectId: task.ProjectId,
            AssignedById: task.AssignedById,
            AssigneeIds: task.AssigneeIds,
            Title: task.Title,
            Description: task.Description,
            UpdatedAt: task.UpdatedAt,
            DueDate: task.DeadlineAt,
            Priority: task.Priority,
            Status: task.Status
        );
        
        await _publishEndpoint.Publish(taskEditedEvent, cancellationToken);

        return new EditTaskCommandResponse
        (
            Id: task.Id,
            AssigneeIds: task.AssigneeIds,
            Title: task.Title,
            Description: task.Description,
            DeadlineAt: task.DeadlineAt,
            Status: task.Status,
            Priority: task.Priority,
            UpdatedAt: task.UpdatedAt
        );
    }

    private bool CanEditTask(string userId, WorkspaceRoles userRole, string assignedById)
    {
        return userRole switch
        {
            WorkspaceRoles.Owner => true,
            WorkspaceRoles.Admin => assignedById == userId,
            _ => false
        };
    }
}
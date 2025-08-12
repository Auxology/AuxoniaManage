using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Features.ProjectTask.Create;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.ProjectTask;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.ProjectTask.CreateTask;

public sealed class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, CreateTaskResponse>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IReadModelRepository _readModelRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateTaskCommandHandler
    (
        IProjectTaskRepository projectTaskRepository,
        IReadModelRepository readModelRepository,
        IWorkspacePermissionService workspacePermissionService,
        IPublishEndpoint publishEndpoint
    )
    {
        _projectTaskRepository = projectTaskRepository;
        _readModelRepository = readModelRepository;
        _workspacePermissionService = workspacePermissionService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CreateTaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));
        Guard.Against.NullOrEmpty(request.AssigneeIds, nameof(request.AssigneeIds));
        Guard.Against.NullOrEmpty(request.Title, nameof(request.Title));
        Guard.Against.NullOrEmpty(request.Description, nameof(request.Description));
        
        if (request.AssigneeIds.Contains(request.UserId))
        {
            throw new CannotAssignTaskToSelfException();
        }
        
        var assigneeIdsList = request.AssigneeIds.ToList();
        
        var project = await _readModelRepository.GetProjectAsync(request.ProjectId, cancellationToken);
        
        if (project == null)
        {
            throw new InvalidProjectIdException();
        }
        
        await _workspacePermissionService
            .EnsureHierarchyAsync
            (
                request.WorkspaceId,
                request.UserId,
                assigneeIdsList,
                cancellationToken
            );
        
        var timeStamp = DateTime.UtcNow;
        
        var projectTask = new Domain.Entities.ProjectTask
        (
            projectId: request.ProjectId,
            assignedById: request.UserId,
            assigneeIds: assigneeIdsList,
            title: request.Title,
            description: request.Description,
            timeStamp: timeStamp,
            deadlineAt: request.DueDate,
            priority: request.Priority,
            status: request.Status
        );
        
        var isSuccess = await _projectTaskRepository.AddAsync(projectTask, cancellationToken);
        
        if (!isSuccess)
        {
            throw new FailedToCreateTaskException();
        }

        var projectTaskCreatedEvent = new ProjectTaskCreatedEvent
        (
            Id: projectTask.Id,
            WorkspaceId: request.WorkspaceId,
            ProjectId: projectTask.ProjectId,
            AssignedById: projectTask.AssignedById,
            AssigneeIds: projectTask.AssigneeIds,
            Title: projectTask.Title,
            Description: projectTask.Description,
            AssignedAt: projectTask.CreatedAt,
            DueDate: projectTask.DeadlineAt,
            Priority: projectTask.Priority,
            Status: projectTask.Status
        );
        
        await _publishEndpoint.Publish(projectTaskCreatedEvent, cancellationToken);
        
        return new CreateTaskResponse
        (
            Id: projectTask.Id,
            ProjectId: projectTask.ProjectId,
            AssignedBy: projectTask.AssignedById,
            AssigneeIds: projectTask.AssigneeIds,
            Title: projectTask.Title,
            Description: projectTask.Description,
            DueDate: projectTask.DeadlineAt ?? DateTime.MinValue,
            Priority: projectTask.Priority,
            Status: projectTask.Status,
            CreatedAt: projectTask.CreatedAt
        );
    }
}
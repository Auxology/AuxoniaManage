using AuxoniaManage.Domain.Enums.Tasks;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.ProjectTask.Edit;

public sealed record EditTaskCommand
(
    Guid Id,
    string UserId,
    Guid WorkspaceId,
    Guid ProjectId,
    IReadOnlyCollection<string> AssigneeIds,
    string? Title,
    string? Description,
    DateTime? DeadlineAt,
    ProjectTaskPriority? Priority,
    ProjectTaskStatus? Status
) : ICommand<EditTaskCommandResponse>;

public sealed record EditTaskCommandResponse
(
    Guid Id,
    IReadOnlyList<string> AssigneeIds,
    string Title,
    string Description,
    DateTime? DeadlineAt,
    ProjectTaskStatus Status,
    ProjectTaskPriority Priority,
    DateTime UpdatedAt
);
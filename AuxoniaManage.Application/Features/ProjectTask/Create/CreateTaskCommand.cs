using AuxoniaManage.Domain.Enums.Tasks;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.ProjectTask.Create;

public sealed record CreateTaskCommand
(
    string UserId,
    Guid WorkspaceId,
    Guid ProjectId,
    IReadOnlyCollection<string> AssigneeIds,
    string Title,
    string Description,
    DateTime? DueDate,
    ProjectTaskPriority Priority,
    ProjectTaskStatus Status
) : ICommand<CreateTaskResponse>;
public sealed record CreateTaskResponse
(
    Guid Id,
    Guid ProjectId,
    string AssignedBy,
    IReadOnlyList<string> AssigneeIds,
    string Title,
    string Description,
    DateTime DueDate,
    ProjectTaskPriority Priority,
    ProjectTaskStatus Status,
    DateTime CreatedAt
);
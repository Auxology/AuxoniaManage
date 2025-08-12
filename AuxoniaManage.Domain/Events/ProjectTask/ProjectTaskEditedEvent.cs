using AuxoniaManage.Domain.Enums.Tasks;

namespace AuxoniaManage.Domain.Events.ProjectTask;

public sealed record ProjectTaskEditedEvent
(
    Guid Id,
    Guid WorkspaceId,
    Guid ProjectId,
    string AssignedById,
    IReadOnlyList<string> AssigneeIds,
    string Title,
    string Description,
    DateTime UpdatedAt,
    DateTime? DueDate,
    ProjectTaskPriority Priority,
    ProjectTaskStatus Status
);
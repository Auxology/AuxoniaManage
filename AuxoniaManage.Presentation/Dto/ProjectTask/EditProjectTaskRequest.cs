using AuxoniaManage.Domain.Enums.Tasks;

namespace AuxoniaManage.Presentation.Dto.ProjectTask;

public sealed record EditProjectTaskRequest
(
    IReadOnlyCollection<string>? AssigneeIds,
    string? Title,
    string? Description,
    DateTime? DeadlineAt,
    ProjectTaskPriority? Priority,
    ProjectTaskStatus? Status
);
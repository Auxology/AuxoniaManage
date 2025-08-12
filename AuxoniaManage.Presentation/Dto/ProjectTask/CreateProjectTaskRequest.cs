using AuxoniaManage.Domain.Enums.Tasks;

namespace AuxoniaManage.Presentation.Dto.ProjectTask;

public sealed record CreateProjectTaskRequest
(
    IReadOnlyCollection<string> AssigneeIds,
    string Title,
    string Description,
    DateTime? DueDate,
    ProjectTaskPriority Priority,
    ProjectTaskStatus Status
);
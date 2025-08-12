using AuxoniaManage.Domain.Enums.Tasks;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.ProjectTask.Get;

public sealed record GetTasksQuery
(
    string UserId,
    Guid WorkspaceId,
    Guid ProjectId
) : IQuery<GetTasksResponse>;

public sealed record ProfileDto
(
    string UserId,
    string FullName,
    string? AvatarUrl
);

public sealed record ProjectTaskDto
(
    Guid Id,
    Guid ProjectId,
    string ProjectName,
    string? ProjectLogoUrl,
    ProfileDto AssignedBy,
    IReadOnlyList<ProfileDto> Assignees,
    string Title,
    string Description,
    DateTime? DueDate,
    ProjectTaskPriority Priority,
    ProjectTaskStatus Status,
    DateTime CreatedAt
);

public sealed record GetTasksResponse
(
    IReadOnlyList<ProjectTaskDto> ProjectTasks
);
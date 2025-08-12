using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.ProjectTask.Delete;

public sealed record DeleteProjectTaskCommand
(
    string UserId,
    Guid Id,
    Guid ProjectId,
    Guid WorkspaceId
) : ICommand<DeleteProjectTaskResponse>;

public sealed record DeleteProjectTaskResponse
(
    Guid Id,
    Guid WorkspaceId,
    Guid ProjectId,
    string DeletedById,
    DateTime DeletedAt
);